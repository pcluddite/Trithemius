using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Trithemius
{
    public partial class EncoderNew : Form
    {
        public EncoderNew()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
        }

        public string message = "";
        int maxSize = 0;
        private void button3_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Image img = Image.FromFile(openFileDialog1.FileName);
                maxSize = ((img.Height - 1) * (img.Width - 1)) / 8;
                label6.Text = "Maximum Avalible Size: " + Program.simplifySize(maxSize
                    * (int)numericUpDown2.Value);
                textBox4.Text = openFileDialog1.FileName;
                img.Dispose();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                Text t = new Text(this);
                t.ShowDialog();
            }
            else
            {
                openFileDialog2.ShowDialog();
            }
            button5_Click(sender, e);
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            button2.Text = radioButton2.Checked ? "Select File" : "Write Text";
        }

        int msgSize = 0;
        private void button5_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                if (message == null) { return; }
                byte[] data = Encoding.ASCII.GetBytes(message);
                if (textBox1.Text.CompareTo("") != 0)
                {
                    data = Crypto.EncryptStringAES(data, textBox1.Text);
                }
                msgSize = data.Length + 4;
            }
            else
            {
                if (openFileDialog2.FileName.CompareTo("") != 0)
                {
                    byte[] data = System.IO.File.ReadAllBytes(openFileDialog2.FileName);
                    if (textBox1.Text.CompareTo("") != 0)
                    {
                        data = Crypto.EncryptStringAES(data, textBox1.Text);
                    }
                    msgSize = data.Length + 4;
                }
            }

            label7.Text = "Space Required: " + Program.simplifySize(
                inflateSize(msgSize) / numericUpDown2.Value
                );
            if (inflateSize(msgSize / (int)numericUpDown2.Value) > maxSize * (int)numericUpDown2.Value)
            {
                label7.Font = new Font(label7.Font, FontStyle.Bold);
                label7.ForeColor = Color.Red;
            }
            else
            {
                label7.Font = new Font(label7.Font, FontStyle.Regular);
                label7.ForeColor = Color.Black;
            }
        }

        private int inflateSize(int size)
        {
            if (textBox2.Text.CompareTo("") != 0)
            {
                Int64 s;
                if (!Int64.TryParse(textBox2.Text, out s))
                {
                    MessageBox.Show("The seed entered is not valid.", Text,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return size;
                }
                int[] seed = Array.ConvertAll<char, int>(
                    textBox2.Text.ToCharArray(), new Converter<char, int>(CharToInt));
                int ln = 0;
                int newSize = 0;
                for (int i = 0; i != size; i++)
                {
                    ln = ln % seed.Length;
                    newSize += seed[ln];
                    ln++;
                }
                return newSize;
            }
            return size;
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            label10.Text = numericUpDown2.Value == 1 ? "Bit" : "Bits";
            label6.Text = "Maximum Avalible Size: " + Program.simplifySize(maxSize
                * (int)numericUpDown2.Value);
            button5_Click(sender, e);
        }

        string IntToString(int i)
        {
            return i.ToString();
        }

        int CharToInt(char c)
        {
            return int.Parse(c.ToString());
        }

        bool error = false;
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Bitmap img = (Bitmap)Bitmap.FromFile(textBox4.Text);
            //Get the pixel intensity used
            int usingARGB = 1;
            Invoke(new MethodInvoker(delegate { usingARGB = comboBox1.SelectedIndex + 1; }));
            //Encode message
            byte[] msg = radioButton1.Checked ?
                Encoding.ASCII.GetBytes(message) :
                System.IO.File.ReadAllBytes(openFileDialog2.FileName);
            //Encrypt message
            if (textBox1.Text.CompareTo("") != 0)
                msg = Crypto.EncryptStringAES(msg, textBox1.Text);
            //Get Seed
            int[] seed = new int[] { 1 };

            if (textBox2.Text.CompareTo("") != 0)
            {
                Int64 s;
                if (!Int64.TryParse(textBox2.Text, out s))
                {
                    error = true;
                    MessageBox.Show("The seed entered is not valid.", Text,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                seed = Array.ConvertAll<char, int>(
                    textBox2.Text.ToCharArray(), new Converter<char, int>(CharToInt));
            }
            //Least significant bits used
            int lsb = (int)numericUpDown2.Value;
            //Append the size of the message to the beginning of the byte array
            byte[] data = new byte[5 + msg.GetUpperBound(0)];
            BitConverter.GetBytes(msg.GetUpperBound(0)).CopyTo(data, 0);
            msg.CopyTo(data, 4);

            //Get the bytes in binary and split it into chunks of the lsb
            string binaryFull = string.Join("",
                    data.Select(l => Convert.ToString(l, 2).PadLeft(8, '0')).ToArray<string>());
            string[] binary = Split(binaryFull, lsb).ToArray();

            int b = 0;

            int pixelsUsed = ((inflateSize(msgSize)/lsb) * (8/lsb)) - 1;
        }

        static IEnumerable<string> Split(string str, int chunkSize)
        {
            return Enumerable.Range(0, str.Length / chunkSize)
                .Select(i => str.Substring(i * chunkSize, chunkSize));
        }

        private void button1_Click(object sender, EventArgs e) {
            if (saveFileDialog2.ShowDialog() == DialogResult.OK) {
                this.Enabled = false;
                this.Text = "Encoding...";
                backgroundWorker1.RunWorkerAsync();
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            this.Enabled = true;
            this.Text = "Encode";
        }
    }
}
