using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Trithemius
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            comboBox2.SelectedIndex = 0;
        }

        int CharToInt(char c)
        {
            return int.Parse(c.ToString());
        }

        bool error = false;
        bool checkSize = false;

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                Bitmap img = (Bitmap)Bitmap.FromFile(textBox4.Text);
                int ARGB = 1;
                Invoke(new MethodInvoker(delegate { ARGB = comboBox2.SelectedIndex + 1; }));
                int lsb = (int)numericUpDown2.Value;
                if (checkBox1.Checked) { lsb = -1; }
                int[] seed = new int[] { 1 };
                if (textBox3.Text.CompareTo("") != 0)
                {
                    Int64 s;
                    if (!Int64.TryParse(textBox3.Text, out s))
                    {
                        error = true;
                        MessageBox.Show("The seed entered is not valid.", Text,
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    seed = Array.ConvertAll<char, int>(
                        textBox3.Text.ToCharArray(), new Converter<char, int>(CharToInt));
                }

                string binary = ProcessBinary(img, ARGB, seed, lsb);

                int size = BitConverter.ToInt32(Program.getByte(binary.Remove(33)), 0) + 1;

                if (size <= 1 || size > (PixelNumber(img.Width - 1, img.Height - 1, img.Width) / 8) - 4)
                {
                    MessageBox.Show("No decodable data was detected.", Text,
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    error = true;
                    return;
                }

                if (checkSize)
                {
                    Invoke(new MethodInvoker(delegate
                    {
                        label6.Text = "Detected Message Size: " +
                            Program.simplifySize(size);
                    }));
                    error = true;
                    return;
                }

                byte[] data = Program.getByte(binary.Remove(0,32)).Take(size).ToArray();

                if (textBox2.Text.CompareTo("") != 0)
                {
                    data = Crypto.DecryptStringAES(data, textBox2.Text);
                }

                if (radioButton1.Checked)
                {
                    Finished f = new Finished(Encoding.ASCII.GetString(data));
                    f.ShowDialog();
                }
                else
                {
                    System.IO.File.WriteAllBytes(saveFileDialog1.FileName, data);
                }
                img.Dispose();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Padding"))
                {
                    MessageBox.Show("The decryption code was most likely invalid.", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                error = true;
            }
        }

        private int PixelNumber(int x, int y, int width)
        {
            return (y * width) + x;
        }

        private Point PixelCoord(int number, int width)
        {
            return new Point(number % width, (number - (number % width)) / width);
        }

        byte[] getPixels(Bitmap img, int ARGB)
        {
            List<byte> data = new List<byte>();
            int size = PixelNumber(img.Width - 1, img.Height - 1, img.Width);
            for (int i = 0; i <= size; i++)
            {
                Point p = PixelCoord(i, img.Width);
                Color c = img.GetPixel(p.X, p.Y);
                byte[] argb = new byte[] { c.A, c.R, c.G, c.B };
                data.Add(argb[ARGB]);
            }
            return data.ToArray();
        }

        private string ProcessBinary(Bitmap img, int ARGB, int[] seed, int lsb)
        {
            List<string> list = new List<string>();
            int ln = 0;
            byte[] pixels = getPixels(img, ARGB);
            int i = seed[0] - 1;
            if (lsb != -1)
            {
                while (i < pixels.Length)
                {
                    list.Add(Convert.ToString(pixels[i], 2).PadLeft(8, '0').Remove(0, 8 - lsb));
                    i += seed[ln % seed.Length];
                    ln++;
                }
            }
            else
            {
                while (i < pixels.Length)
                {
                    list.Add(
                        Convert.ToString(pixels[i], 2).PadLeft(8, '0').Remove(0, 7) == "1" ? "0" : "1"
                        );
                    i += seed[ln % seed.Length];
                    ln++;
                }
            }

            return string.Join("", list.ToArray());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            if (radioButton2.Checked)
            {
                if (saveFileDialog1.ShowDialog() != DialogResult.OK)
                {
                    this.Enabled = true;
                    return;
                }
            }
            backgroundWorker2.RunWorkerAsync();
        }

        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Enabled = true;
            checkSize = false;
            if (error) { error = false; return; }

            if (radioButton2.Checked &&
                MessageBox.Show(this, "Decoding Completed!\r\nWould you like to open the file?",
                    Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    System.Diagnostics.Process.Start(saveFileDialog1.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            this.Enabled = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox4.Text = openFileDialog1.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            checkSize = true;
            backgroundWorker2.RunWorkerAsync();
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            label10.Text = numericUpDown2.Value == 1 ? "Bit" : "Bits";
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            label10.Enabled = label9.Enabled = numericUpDown2.Enabled =
                !checkBox1.Checked;
        }
    }
}
