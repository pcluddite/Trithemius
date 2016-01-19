using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Trithemius
{
    public partial class EncodeForm : Form
    {
        public EncodeForm()
        {
            InitializeComponent();
        }

        public string message = null;

        private void LockWindow()
        {
            Enabled = false;
            Text = "Encoding...";
        }

        private void UnlockWindow()
        {
            Enabled = true;
            Text = "Encoder";
        }

        private void ShowError(Exception ex)
        {
            MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void encodeButton_Click(object sender, EventArgs e)
        {
            if (imageSaveDialog.ShowDialog() != DialogResult.OK) {
                return;
            }

            LockWindow();

            try {
                Trithemius t = new Trithemius((Bitmap)Bitmap.FromFile(pathTextbox.Text));
                t.Color = (PixelColor)(pixelValueComboBox.SelectedIndex + 1);
                t.LeastSignificantBits = (int)bitsNumericUpDown.Value;

                byte[] msg;
                if (textRadioButton.Checked) {
                    msg = Encoding.ASCII.GetBytes(message);
                }
                else {
                    msg = File.ReadAllBytes(msgOpenDialog.FileName);
                }

                if (!passwordBox.Text.Equals("")) {
                    msg = Crypto.EncryptStringAES(msg, passwordBox.Text);
                }

                if (!seedBox.Text.Equals("")) {
                    t.Seed = new TrithemiusSeed(seedBox.Text);
                }


                encodeWorker.RunWorkerAsync(new object[] { t, msg });
            }
            catch(FileNotFoundException ex) {
                ShowError(ex);
                UnlockWindow();
            }
        }

        private void encodeWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try {
                object[] args = (object[])e.Argument;
                Trithemius t = (Trithemius)args[0];
                byte[] msg = (byte[])args[1];

                Dictionary<int, byte> changes = t.Encode(msg, imageSaveDialog.FileName);

                if (checkBox1.Checked) {
                    Invoke(new MethodInvoker(delegate { SaveLog((int)t.Color, changes, Trithemius.PixelNumber(t.BitmapImage.Width, t.BitmapImage.Height, t.BitmapImage.Width)); }));
                }
                t.Dispose();
                e.Result = true;
            }
            catch (Exception ex) {
                ShowError(ex);
                e.Result = false;
            }
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

        private void Form1_Load(object sender, EventArgs e)
        {
            pixelValueComboBox.SelectedIndex = 0;
        }

        int maxSize = 0;

        private void browseButton_Click(object sender, EventArgs e)
        {
            if (imageOpenDialog.ShowDialog() == DialogResult.OK)
            {
                Image img = Image.FromFile(imageOpenDialog.FileName);
                maxSize = ((img.Height - 1) * (img.Width - 1)) / 8;
                label6.Text = "Maximum Avalible Size: " + Program.SizeToString(maxSize
                    * (int)bitsNumericUpDown.Value);
                pathTextbox.Text = imageOpenDialog.FileName;
                img.Dispose();
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            writeTextButton.Text = fileRadioButton.Checked ? "Select File" : "Write Text";
        }

        private void encodeWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            UnlockWindow();

            if ((bool)e.Result) {
                MessageBox.Show(this, "Encoding completed! You should spot no visible image differences.", Text,
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void writeTextButton_Click(object sender, EventArgs e)
        {
            if (textRadioButton.Checked) {
                Text t = new Text(this);
                t.ShowDialog();
            }
            else {
                msgOpenDialog.ShowDialog();
            }
            refreshButton_Click(sender, e);
        }

        private void refreshButton_Click(object sender, EventArgs e)
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

                if (!passwordBox.Text.Equals(""))
                    msg = Crypto.EncryptStringAES(msg, passwordBox.Text);

                int msgSize = t.GetRequiredSize(msg);

                label7.Text = "Space Required: " + Program.SizeToString(msgSize);
                if (msgSize > maxSize) {
                    label7.Font = new Font(label7.Font, FontStyle.Bold);
                    label7.ForeColor = Color.Red;
                }
                else {
                    label7.Font = new Font(label7.Font, FontStyle.Regular);
                    label7.ForeColor = Color.Black;
                }
            }
            catch (FileNotFoundException ex) {
                ShowError(ex);
            }
        }
        
        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            label10.Text = bitsNumericUpDown.Value == 1 ? "Bit" : "Bits";
            label6.Text = "Maximum Avalible Size: " + Program.SizeToString(maxSize
                * (int)bitsNumericUpDown.Value);
            refreshButton_Click(sender, e);
        }

        private void randomButton_Click(object sender, EventArgs e)
        {
            seedBox.Text = TrithemiusSeed.RandomSeed((int)seedSize.Value).ToString();
        }

        private void seedBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != '\b') {
                e.Handled = true;
            }
        }
    }
}