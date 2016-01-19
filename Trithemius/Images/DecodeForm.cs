using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Trithemius
{
    public partial class DecodeForm : Form
    {
        private Finished f;

        public DecodeForm()
        {
            InitializeComponent();
            pixelValueComboBox.SelectedIndex = 0;
        }

        private void LockWindow()
        {
            Enabled = false;
            Text = "Decoding...";
        }

        private void UnlockWindow()
        {
            Enabled = true;
            Text = "Decoder";
        }

        private void decodeButton_Click(object sender, EventArgs e)
        {
            LockWindow();
            if (fileRadioButton.Checked) {
                if (saveFileDialog.ShowDialog() != DialogResult.OK) {
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

        private Trithemius MakeTrithemius()
        {
            Trithemius t = new Trithemius((Bitmap)Bitmap.FromFile(pathTextbox.Text));
            t.Color = (PixelColor)(pixelValueComboBox.SelectedIndex + 1);

            if (!seedBox.Text.Equals("")) {
                t.Seed = new TrithemiusSeed(seedBox.Text);
            }

            t.LeastSignificantBits = (int)bitsNumericUpDown.Value;

            return t;
        }

        private void checkSizeButton_Click(object sender, EventArgs e)
        {
            LockWindow();

            try {
                decodeWorker.RunWorkerAsync(new object[] { true, MakeTrithemius(), passwordBox.Text });
            }
            catch (FileNotFoundException ex) {
                UnlockWindow();
                ShowError(ex);
            }
        }

        private void decodeWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                object[] args = (object[])e.Argument;

                bool checkSize = (bool)args[0];
                Trithemius t = (Trithemius)args[1];
                string password = (string)args[2];

                if (checkSize)
                {
                    int size = t.CheckSize();
                    if (size < 0) {
                        ErrorNoData();
                        e.Result = false;
                        return;
                    }
                    Invoke(new MethodInvoker(delegate
                    {
                        label6.Text = "Detected Message Size: " +
                            Program.SizeToString(size);
                    }));
                    e.Result = true;
                    return;
                }

                byte[] data = t.Decode();

                if (data == null) {
                    ErrorNoData();
                    e.Result = false;
                    return;
                }

                if (!password.Equals("")) {
                    data = Crypto.DecryptStringAES(data, password);
                }

                if (textRadioButton.Checked) {
                    f = new Finished(Encoding.ASCII.GetString(data));
                    f.ShowDialog();
                }
                else {
                    File.WriteAllBytes(saveFileDialog.FileName, data);
                }

                t.Dispose();
                e.Result = true;
            }
            catch (Exception ex) {
                if (ex.Message.Contains("Padding")) {
                    ShowErrorT("The decryption code was most likely invalid.");
                }
                else {
                    ShowErrorT(ex);
                }
                e.Result = false;
            }
        }
        
        private void decodeWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            UnlockWindow();

            if (!(bool)e.Result) {
                return;
            }

            if (fileRadioButton.Checked &&
                MessageBox.Show(this, "Decoding Completed!\r\nWould you like to open the file?",
                    Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                try {
                    System.Diagnostics.Process.Start(saveFileDialog.FileName);
                }
                catch (Exception ex) {
                    ShowError(ex);
                }
            }
        }

        private void browseButton_Click(object sender, EventArgs e)
        {
            if (imageOpenDialog.ShowDialog() == DialogResult.OK)
            {
                pathTextbox.Text = imageOpenDialog.FileName;
            }
        }

        private void bitsNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            label10.Text = bitsNumericUpDown.Value == 1 ? "Bit" : "Bits";
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            /*label10.Enabled = label9.Enabled = numericUpDown2.Enabled =
                !checkBox1.Checked;*/
        }

        private void Form2_Activated(object sender, EventArgs e)
        {
            if (!this.Enabled && f != null)
                f.Activate();
        }

        private void ErrorNoData()
        {
            ShowErrorT("No decodable data was detected.");
        }

        private void ShowErrorT(Exception ex)
        {
            ShowErrorT(ex.Message + "\n" + ex.TargetSite);
        }

        private void ShowErrorT(string message)
        {
            MessageBox.Show(message, Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void ShowError(Exception ex)
        {
            ShowError(ex.Message + "\n" + ex.TargetSite);
        }

        private void ShowError(string message)
        {
            MessageBox.Show(this, message, Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void seedBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != '\b') {
                e.Handled = true;
            }
        }
    }
}
