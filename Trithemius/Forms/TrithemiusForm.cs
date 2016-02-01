/**
 *  Trithemius
 *  Copyright (C) 2012-2016 Timothy Baxendale
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
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Diagnostics;
using Encryption;
using Trithemius.Properties;

namespace Trithemius
{
    public partial class TrithemiusForm : Form
    {
        private string message = "";
        private double maxSize = 0;
        private Size dimensions;

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
                ShowError("You must specify an image file.");
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
                Trithemius t = MakeTrithemius();

                byte[] msg;
                if (textRadioButton.Checked) {
                    msg = Encoding.UTF8.GetBytes(message);
                }
                else {
                    msg = File.ReadAllBytes(msgOpenDialog.FileName);
                }
                
                
                encodeWorker.RunWorkerAsync(new object[] { t, msg, passwordBox.Text });
            }
            catch(FileNotFoundException ex) {
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
            try {
                object[] args = (object[])e.Argument;
                Trithemius t = (Trithemius)args[0];
                byte[] msg = (byte[])args[1];
                string pass = (string)args[2];

                if (!string.IsNullOrEmpty(pass))
                    msg = AESThenHMAC.SimpleEncryptWithPassword(msg, pass);

                t.Encode(msg, imageSaveDialog.FileName);
                
                t.Dispose();
                e.Result = new object[] { true };
            }
            catch (Exception ex) {
                e.Result = new object[] { false, ex.Message };
            }
        }

        private void decodeWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try {
                object[] args = (object[])e.Argument;

                bool checkSize = (bool)args[0];
                Trithemius t = (Trithemius)args[1];
                string pass = (string)args[2];
                
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

                t.Dispose();
            }
            catch (Exception ex) {
                e.Result = new object[] { false, ex.Message };
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
                    try {
                        Process.Start(msgSaveDialog.FileName);
                    }
                    catch (Exception ex) {
                        ShowError(ex);
                    }
                }
            }
            else {
                ShowError((string)result[1]);
            }
        }

        private Trithemius MakeTrithemius()
        {
            Trithemius t = new Trithemius(OpenBitmap(pathTextbox.Text));
            t.Color = (PixelColor)(pixelValueComboBox.SelectedIndex + 1);
            t.InvertBits = invertBox.Checked;

            if (!string.IsNullOrEmpty(seedBox.Text)) {
                t.Seed = new TrithemiusSeed(seedBox.Text);
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
            availableSizeBox.Text = Program.SizeToString(maxSize);
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
                ShowError((string)result[1]);
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
            try {
                Trithemius t = new Trithemius(null);
                t.LeastSignificantBits = (int)bitsNumericUpDown.Value;

                byte[] msg;
                if (textRadioButton.Checked) {
                    if (message == null)
                        return;
                    msg = Encoding.ASCII.GetBytes(message);
                }
                else {
                    if (msgOpenDialog.FileName.Equals(""))
                        return;
                    msg = File.ReadAllBytes(msgOpenDialog.FileName);
                }

                if (!string.IsNullOrEmpty(passwordBox.Text))
                    msg = AESThenHMAC.SimpleEncryptWithPassword(msg, passwordBox.Text);

                int msgSize = t.GetRequiredSize(msg);

                requiredSizeBox.Text = Program.SizeToString(msgSize);
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
            ShowError(ex.Message);
        }

        private void ShowError(string message)
        {
            MessageBox.Show(this, message, Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void randomButton_Click(object sender, EventArgs e)
        {
            RandomSeed randForm = new RandomSeed();
            if (randForm.ShowDialog() == DialogResult.OK)
                seedBox.Text = TrithemiusSeed.RandomSeed(randForm.Result).ToString();
        }
    }
}