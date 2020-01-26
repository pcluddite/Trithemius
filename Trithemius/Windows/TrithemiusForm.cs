/**
 *  Trithemius
 *  Copyright (C) Timothy Baxendale
 *
 *  This program is free software; you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation; either version 2 of the License, or
 *  (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License along
 *  with this program; if not, write to the Free Software Foundation, Inc.,
 *  51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA.
**/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Security;
using System.Text;
using System.Windows.Forms;
using Monk.Encryption;
using Monk.Imaging;

namespace Trithemius.Windows
{
    public partial class TrithemiusForm : Form
    {
        private string message = "";
        private double maxSize = 0;
        private Size dimensions;
        private int previous_rand = 5;

        public TrithemiusForm()
        {
            InitializeComponent();
        }

        private void TrithemiusForm_Load(object sender, EventArgs e)
        {
            pixelValueComboBox.SelectedIndex = 0;
        }

        private void LockWindow()
        {
            Enabled = false;
            Text = encodeRadio.Checked ? "Encoding..." : "Decoding...";
        }

        private void UnlockWindow()
        {
            Enabled = true;
            Text = "Trithemius";
        }
        
        /// <summary>
        /// Loads a bitmap into memory so the original file can be saved over
        /// </summary>
        /// <param name="path">the bitmap path</param>
        /// <returns></returns>
        public Bitmap OpenBitmap(string path)
        {
            MemoryStream stream = new MemoryStream();
            using (Image image = Image.FromFile(path)) {
                image.Save(stream, ImageFormat.Png);
            }
            return (Bitmap)Image.FromStream(stream);
        }

        private bool CheckImagePath()
        {
            if (pathTextbox.Text == "") {
                Program.ShowError(this, "You must specify an image file.");
                return false;
            }
            return true;
        }

        private void encodeButton_Click(object sender, EventArgs e)
        {
            if (!CheckImagePath())
                return;

            if (imageSaveDialog.ShowDialog() != DialogResult.OK)
                return;

            LockWindow();

            try {
                Steganographer t = MakeTrithemius();

                byte[] msg;
                if (textRadioButton.Checked) {
                    msg = Encoding.UTF8.GetBytes(message);
                }
                else {
                    msg = File.ReadAllBytes(msgOpenDialog.FileName);
                }
                
                encodeWorker.RunWorkerAsync(new object[] { t, msg, passwordBox.Text });
            }
            catch(Exception ex) when (ex is EncoderFallbackException || ex is IOException || ex is SecurityException) {
                ShowError(ex);
                UnlockWindow();
            }
        }

        private void decodeButton_Click(object sender, EventArgs e)
        {
            if (!CheckImagePath())
                return;

            LockWindow();
            if (fileRadioButton.Checked) {
                if (msgSaveDialog.ShowDialog() != DialogResult.OK) {
                    UnlockWindow();
                    return;
                }
            }
            try {
                decodeWorker.RunWorkerAsync(new object[] { false, MakeTrithemius(), passwordBox.Text });
            }
            catch (FileNotFoundException ex) {
                UnlockWindow();
                ShowError(ex);
            }
        }

        private void encodeWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            object[] args = (object[])e.Argument;
            Steganographer t = (Steganographer)args[0];
            byte[] msg = (byte[])args[1];
            string pass = (string)args[2];

            try {
                if (!string.IsNullOrEmpty(pass))
                    msg = AESThenHMAC.SimpleEncryptWithPassword(msg, pass);

                t.Encode(msg, imageSaveDialog.FileName);
                
                e.Result = new object[] { true };
            }
            catch (Exception ex) {
                e.Result = new object[] { false, ex.Message };
            }
            finally {
                t.Dispose();
            }
        }

        private void decodeWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            object[] args = (object[])e.Argument;
            bool checkSize = (bool)args[0];
            Steganographer t = (Steganographer)args[1];
            string pass = (string)args[2];

            try {
                byte[] msg = t.Decode();

                if (msg == null) {
                    e.Result = new object[] { false, "No decodable data was detected." };
                    return;
                }

                if (!string.IsNullOrEmpty(pass)) {
                    msg = AESThenHMAC.SimpleDecryptWithPassword(msg, pass);
                    if (msg == null) {
                        e.Result = new object[] { false, "The encryption key was invalid." };
                        return;
                    }
                }

                if (textRadioButton.Checked) {
                    e.Result = new object[] { true, Encoding.UTF8.GetString(msg) };
                }
                else {
                    File.WriteAllBytes(msgSaveDialog.FileName, msg);
                    e.Result = new object[] { true };
                }
            }
            catch (Exception ex) {
                e.Result = new object[] { false, ex.Message };
            }
            finally {
                t.Dispose();
            }
        }

        private void decodeWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            UnlockWindow();

            object[] result = (object[])e.Result;

            if ((bool)result[0]) {
                if (textRadioButton.Checked) {
                    Finished finished = new Finished((string)result[1]);
                    finished.ShowDialog();
                }
                else if (MessageBox.Show(this, "Decoding Completed!\r\nWould you like to open the file?",
                        Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                    Program.TryStart(this, new ProcessStartInfo(msgSaveDialog.FileName));
                }
            }
            else {
                Program.ShowError(this, (string)result[1]);
            }
        }

        private Steganographer MakeTrithemius()
        {
            Steganographer t = new Steganographer(OpenBitmap(pathTextbox.Text));
            t.Color = (PixelColor)(pixelValueComboBox.SelectedIndex + 1);
            t.InvertBits = invertBox.Checked;

            if (!string.IsNullOrEmpty(seedBox.Text)) {
                t.Seed = new Seed(seedBox.Text);
            }

            t.LeastSignificantBits = (int)bitsNumericUpDown.Value;

            return t;
        }

        private void SaveLog(int RGB, Dictionary<int, byte> changes, int size)
        {
            if (changesSaveDialog.ShowDialog() == DialogResult.OK)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("[PROPERTIES]");
                sb.AppendLine(string.Format("COLOR={0}", RGB));
                sb.AppendLine(string.Format("SIZE={0}", size));
                sb.AppendLine("\r\n[ORIGINAL]");
                foreach (KeyValuePair<int, byte> kv in changes)
                {
                    sb.AppendLine(string.Format("{0}={1}", kv.Key, kv.Value));
                }
                File.WriteAllText(changesSaveDialog.FileName, sb.ToString());
            }
        }

        private void browseButton_Click(object sender, EventArgs e)
        {
            if (imageOpenDialog.ShowDialog() == DialogResult.OK)
            {
                try {
                    using (Image img = Image.FromFile(imageOpenDialog.FileName)) {
                        dimensions = new Size(img.Width - 1, img.Height - 1);
                        pathTextbox.Text = imageOpenDialog.FileName;
                    }
                    RefreshAvailableSize();
                }
                catch (FileNotFoundException ex) {
                    ShowError(ex);
                }
            }
        }

        private void RefreshAvailableSize()
        {
            maxSize = ((dimensions.Width * dimensions.Height) / 8) * (int)bitsNumericUpDown.Value;
            availableSizeBox.Text = SizeToString(maxSize);
        }

        private static string SizeToString(double size)
        {
            int unit = 0;
            if (size >= 1024) { size /= 1024; unit = 1; }
            if (size >= 1024) { size /= 1024; unit = 2; }
            if (size >= 1024) { size /= 1024; unit = 3; }
            string n = size.ToString("0.##");
            switch (unit) {
                case 0: n += " B"; break;
                case 1: n += " KB"; break;
                case 2: n += " MB"; break;
                case 3: n += " GB"; break;
            }
            return n;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            writeTextButton.Text = fileRadioButton.Checked ? "Select File" : "Write Text";
        }

        private void encodeWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            UnlockWindow();

            object[] result = (object[])e.Result;

            if ((bool)result[0]) {
                MessageBox.Show(this, "Encoding completed! You should spot no visible image differences.", Text,
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else {
                Program.ShowError(this, (string)result[1]);
            }
        }

        private void writeTextButton_Click(object sender, EventArgs e)
        {
            if (textRadioButton.Checked) {
                Text t = new Text(message);
                if (t.ShowDialog() == DialogResult.OK)
                    message = t.Result;
            }
            else {
                msgOpenDialog.ShowDialog();
            }
            RefreshRequiredSize();
        }

        private void RefreshRequiredSize()
        {
            Steganographer t = new Steganographer((Bitmap)null);
            t.LeastSignificantBits = (int)bitsNumericUpDown.Value;

            try {    
                byte[] msg;
                if (textRadioButton.Checked) {
                    if (message == null)
                        return;
                    msg = Encoding.UTF8.GetBytes(message);
                }
                else {
                    if (string.IsNullOrEmpty(msgOpenDialog.FileName))
                        return;
                    msg = File.ReadAllBytes(msgOpenDialog.FileName);
                }

                if (!string.IsNullOrEmpty(passwordBox.Text))
                    msg = AESThenHMAC.SimpleEncryptWithPassword(msg, passwordBox.Text);

                int msgSize = t.GetRequiredSize(msg);

                requiredSizeBox.Text = SizeToString(msgSize);
                if (msgSize > maxSize) {
                    requiredSizeLabel.ForeColor = Color.Red;
                }
                else {
                    requiredSizeLabel.ForeColor = Color.Black;
                }
            }
            catch (FileNotFoundException ex) {
                ShowError(ex);
            }
            finally {
                t.Dispose();
            }
        }
        
        private void bitsNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            RefreshAvailableSize();
        }

        private void seedBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != '\b') {
                e.Handled = true;
            }
        }

        private void encodeRadio_CheckedChanged(object sender, EventArgs e)
        {
            if (encodeRadio.Checked) {
                encodeButton.Text = "&Encode";
                textRadioButton.Text = "Encode Text";
                fileRadioButton.Text = "Encode File";
                writeTextButton.Visible =  requiredSizeLabel.Visible = requiredSizeBox.Visible = true;
                encodeButton.Click -= decodeButton_Click;
                encodeButton.Click += encodeButton_Click;
            }
            else {
                encodeButton.Text = "D&ecode";
                textRadioButton.Text = "Decode Text";
                fileRadioButton.Text = "Decode File";
                writeTextButton.Visible = requiredSizeLabel.Visible = requiredSizeBox.Visible = false;
                encodeButton.Click -= encodeButton_Click;
                encodeButton.Click += decodeButton_Click;
            }
        }

        private void ShowError(Exception ex)
        {
            Program.ShowError(this, ex.Message);
        }

        private void randomButton_Click(object sender, EventArgs e)
        {
            RandomSeed randForm = new RandomSeed(previous_rand);
            if (randForm.ShowDialog() == DialogResult.OK) {
                previous_rand = randForm.Result;
                seedBox.Text = Seed.RandomSeed(previous_rand).ToString();
            }
        }
    }
}