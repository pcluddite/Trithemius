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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        bool error = false;
        public string message = null;

        string IntToString(int i)
        {
            return i.ToString();
        }

        int CharToInt(char c)
        {
            return int.Parse(c.ToString());
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                Bitmap img = (Bitmap)Bitmap.FromFile(textBox4.Text);
                int ARGB = 1;
                Invoke(new MethodInvoker(delegate { ARGB = comboBox1.SelectedIndex + 1; }));
                byte[] msg = radioButton1.Checked ?
                    Encoding.ASCII.GetBytes(message) :
                    System.IO.File.ReadAllBytes(openFileDialog2.FileName);

                if (textBox1.Text.CompareTo("") != 0)
                {
                    msg = Crypto.EncryptStringAES(msg, textBox1.Text);
                }

                string binary = "";

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

                byte[] data = new byte[5 + msg.GetUpperBound(0)];
                BitConverter.GetBytes(msg.GetUpperBound(0)).CopyTo(data, 0);
                msg.CopyTo(data, 4);
                binary = string.Join("",
                    data.Select(l => Convert.ToString(l, 2).PadLeft(8, '0')).ToArray<string>());

                int b = 0;
                int i = seed[0];

                List<byte> nData = new List<byte>();
                Color[] allPixels = getPixels(img, ARGB);
                Dictionary<int, byte> changes = new Dictionary<int, byte>();
                int ln = 0;
                foreach (char c in binary)
                {
                    int[] color = new int[] { allPixels[i - 1].A, 
                        allPixels[i - 1].R, allPixels[i - 1].G, allPixels[i - 1].B };
                    int argb = color[ARGB];

                    b = c == '1' ?
                        (argb & 1) != 0 ?
                        argb == 255 ? 254 : argb + 1 : argb
                        : (argb & 1) == 0 ? argb + 1 : argb;
                    if (b != argb) { changes.Add(i - 1, (byte)argb); }
                    color[ARGB] = b;
                    allPixels[i - 1] = Color.FromArgb(color[0], color[1], color[2], color[3]);
                    i += seed[ln % seed.Length];
                    ln++;
                }
                img = setPixels(img, allPixels, seed);
                int width = img.Width, height = img.Height;
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                img.Dispose();
                Bitmap.FromStream(ms).Save(saveFileDialog2.FileName, System.Drawing.Imaging.ImageFormat.Png);
                if (checkBox1.Checked)
                {
                    Invoke(new MethodInvoker(delegate { savelog(ARGB, changes, PixelNumber(width, height, width)); }));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message +
                    ex.TargetSite
                    , Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                error = true;
            }
        }

        Color[] getPixels(Bitmap img, int ARGB)
        {
            List<Color> data = new List<Color>();
            int size = PixelNumber(img.Width - 1, img.Height - 1, img.Width);
            for (int i = 0; i <= size; i++)
            {
                Point p = PixelCoord(i, img.Width);
                data.Add(img.GetPixel(p.X, p.Y));
            }
            return data.ToArray();
        }

        private Bitmap setPixels(Bitmap img, Color[] pixels, int[] seed)
        {
            for (int i = 0; i < pixels.Length; i++)
            {
                Point p = PixelCoord(i, img.Width);
                img.SetPixel(p.X, p.Y, pixels[i]);
            }
            return img;
        }

        private int PixelNumber(int x, int y, int width)
        {
            return (y * width) + x;
        }

        private Point PixelCoord(int number, int width)
        {
            return new Point(number % width, (number - (number % width)) / width);
        }

        void savelog(int RGB, Dictionary<int, byte> changes, int size)
        {
            if (saveFileDialog3.ShowDialog() == DialogResult.OK)
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
                System.IO.File.WriteAllText(saveFileDialog3.FileName, sb.ToString());
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
        }

        int maxSize = 0;

        private void button3_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Image img = Image.FromFile(openFileDialog1.FileName);
                maxSize = ((img.Height - 1) * (img.Width - 1)) / 8;
                label6.Text = "Maximum Avalible Size: " + Program.simplifySize(maxSize);
                textBox4.Text = openFileDialog1.FileName;
                img.Dispose();
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            button2.Text = radioButton2.Checked ? "Select File" : "Write Text";
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Enabled = true;
            Text = "Encoder";
            if (error) { error = false; return; }
                MessageBox.Show(this, "Encoding completed! You should spot no visible image differences.", Text,
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        int[] getSeed(int ln)
        {
            List<int> l = new List<int>();
            Random r = new Random();
            for (int i = 0; i != ln; i++)
            {
                l.Add(r.Next(1, 10));
            }
            return l.ToArray();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (saveFileDialog2.ShowDialog() == DialogResult.OK)
            {
                Enabled = false;
                Text = "Encoding...";
                backgroundWorker1.RunWorkerAsync();
            }
        }

        int msgSize = 0;

        private void button2_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                Text t = new Text(this);
                t.ShowDialog();
                if (message == null) { return; }
                byte[] data = Encoding.ASCII.GetBytes(message);
                if (textBox1.Text.CompareTo("") != 0)
                {
                    data = Crypto.EncryptStringAES(data, textBox1.Text);
                }
                msgSize = data.Length + 32;
            }
            else
            {
                if (openFileDialog2.ShowDialog() == DialogResult.OK)
                {
                    byte[] data = System.IO.File.ReadAllBytes(openFileDialog2.FileName);
                    if (textBox1.Text.CompareTo("") != 0)
                    {
                        data = Crypto.EncryptStringAES(data, textBox1.Text);
                    }
                    msgSize = data.Length + 32;
                }
            }
            label7.Text = "Space Required: " + Program.simplifySize(inflateSize(msgSize));
            if (inflateSize(msgSize) > maxSize)
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

        private void button4_Click(object sender, EventArgs e)
        {
            textBox2.Text =
            string.Join("",
            Array.ConvertAll<int, string>(
                getSeed((int)numericUpDown1.Value), new Converter<int,string>(IntToString)));
        }

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
                msgSize = data.Length + 32;
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
                    msgSize = data.Length + 32;
                }
            }

            label7.Text = "Space Required: " + Program.simplifySize(
                inflateSize(msgSize)
                );
            if (inflateSize(msgSize) > maxSize)
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
    }
}
