using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Trithemius
{
    public partial class DecoderNew : Form
    {
        public DecoderNew()
        {
            InitializeComponent();
            comboBox2.SelectedIndex = 0;
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

        private void button3_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox4.Text = openFileDialog1.FileName;
            }
        }

        bool checkSize = false;
        private void button2_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            checkSize = true;
            backgroundWorker2.RunWorkerAsync();
        }

        bool error = false;
        Finished f;
        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            Bitmap img = (Bitmap)Bitmap.FromFile(textBox4.Text);
            int currentARGB = 1;
            Invoke(new MethodInvoker(delegate { currentARGB = comboBox2.SelectedIndex + 1; }));
            int lsb = -1;
            if (!checkBox1.Checked)
                lsb = (int)numericUpDown2.Value;
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

            int size = BitConverter.ToInt32(
                Program.getByte(readBits(img, currentARGB, 32, lsb, seed
                )), 0) + 1;

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
            string bits;
            byte[] data = Program.getByte((bits = readBits(img, currentARGB, (size * 8) + 32, lsb, seed)));
            data = data.Skip(4).ToArray();

            if (textBox2.Text.CompareTo("") != 0)
            {
                data = Crypto.DecryptStringAES(data, textBox2.Text);
            }

            if (radioButton1.Checked)
            {
                f = new Finished(Encoding.ASCII.GetString(data));
                f.ShowDialog();
            }
            else
            {
                System.IO.File.WriteAllBytes(saveFileDialog1.FileName, data);
            }
            img.Dispose();
        }

        private int PixelNumber(int x, int y, int width)
        {
            return (y * width) + x;
        }

        int CharToInt(char c)
        {
            return int.Parse(c.ToString());
        }

        private string readBits(Bitmap img, int ARGB, int end, int lsb, int[] seed)
        {
            StringBuilder bits = new StringBuilder();
            int ln = 0;
            for (int i = seed[0] - 1; bits.Length < end; i += seed[ln % seed.Length])
            {
                Point p = PixelCoord(i, img.Width);
                Color c = img.GetPixel(p.X, p.Y);
                int[] cARGB = new int[] { c.A, c.R, c.G, c.B };

                bits.Append(Convert.ToString(cARGB[ARGB], 2)
                    .PadLeft(8, '0').Remove(0, 8 - lsb));
                ln++;
            }
            return bits.ToString().Substring(0, end);
        }

        private Point PixelCoord(int number, int width)
        {
            return new Point(number % width, (number - (number % width)) / width);
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
    }
}
