using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ImageSteganography
{
    public partial class Full : Form
    {
        public Full()
        {
            InitializeComponent();
        }

        private void Full_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            comboBox4.SelectedIndex = 0;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            label7.Enabled = textBox2.Enabled = !radioButton2.Checked;
            label1.Enabled = textBox3.Enabled = button4.Enabled = radioButton2.Checked;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox4.Text = openFileDialog1.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (saveFileDialog2.ShowDialog() == DialogResult.OK)
            {
                textBox5.Text = saveFileDialog2.FileName;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            tabControl1.Enabled = false;
            backgroundWorker1.RunWorkerAsync();
            Text = "Encoding...";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                textBox3.Text = openFileDialog2.FileName;
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

        #region Encoding

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            tabControl1.Enabled = true;
            Text = "The Steganographer";
            if (error) { error = false; return; }
            if (checkBox4.Checked && tabControl1.SelectedTab.Text.StartsWith("D"))
            {
                MessageBox.Show(this, "Image Restoration completed.", Text,
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(this, "Encoding completed! You should spot no visible image differences.", Text,
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        bool error = false;

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                Bitmap img = (Bitmap)Bitmap.FromFile(textBox4.Text);
                int ARGB = 1;
                Invoke(new MethodInvoker(delegate { ARGB = comboBox1.SelectedIndex + 1; }));
                byte[] msg = !radioButton2.Checked ?
                    Encoding.ASCII.GetBytes(textBox2.Text) :
                    System.IO.File.ReadAllBytes(textBox3.Text);

                if (textBox1.Text.CompareTo("") != 0) {
                    msg = Crypto.EncryptStringAES(msg, textBox1.Text);
                }

                string binary = "";


                byte[] data = new byte[5 + msg.GetUpperBound(0)];
                BitConverter.GetBytes(msg.GetUpperBound(0)).CopyTo(data, 0);
                msg.CopyTo(data, 4);
                binary = string.Join("",
                    data.Select(l => Convert.ToString(l, 2).PadLeft(8, '0')).ToArray<string>());

                int b = 0;
                int i = 1;

                List<byte> nData = new List<byte>();
                byte[] allPixels = getPixels(img, binary.Length, ARGB);
                Dictionary<int, byte> changes = new Dictionary<int, byte>();
                foreach (char c in binary)
                {
                    int argb = allPixels[i];

                    b = c == '1' ?
                        (argb & 1) != 0 ?
                        argb == 255 ? 254 : argb + 1 : argb
                        : (argb & 1) == 0 ? argb + 1 : argb;
                    nData.Add((byte)b);
                    if (b != argb) { changes.Add(i, (byte)argb); }
                    i++;
                }
                img = setPixels(img, nData.ToArray(), ARGB);
                int width = img.Width, height = img.Height;
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                img.Dispose();
                Bitmap.FromStream(ms).Save(textBox5.Text, System.Drawing.Imaging.ImageFormat.Png);
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

        byte[] getPixels(Bitmap img, int size, int ARGB)
        {
            List<byte> data = new List<byte>();
            for (int i = 0; i <= size; i++)
            {
                Point p = PixelCoord(i, img.Width);
                Color c = img.GetPixel(p.X, p.Y);
                byte[] argb = new byte[] { c.A, c.R, c.G, c.B };
                data.Add(argb[ARGB]);
            }
            return data.ToArray();
        }

        private Bitmap setPixels(Bitmap img, byte[] pixels, int ARGB)
        {
            int i = 0;
            foreach (byte b in pixels)
            {
                Point p = PixelCoord(i, img.Width);
                Color col = img.GetPixel(p.X, p.Y);
                int[] argb = new int[] { col.A, col.R, col.G, col.B };
                argb[ARGB] = b;
                img.SetPixel(p.X, p.Y, Color.FromArgb(argb[0], argb[1], argb[2], argb[3]));
                i++;
            }
            return img;
        }

        #endregion

        #region Decoding
        
        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            tabControl1.Enabled = true;
            Text = "The Steganographer";

            if (error) { error = false; return; }

            if (radioButton7.Checked &&
                MessageBox.Show(this, "Decoding Completed!\r\nWould you like to open the file?",
                    Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    System.Diagnostics.Process.Start(textBox7.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                Bitmap img = (Bitmap)Bitmap.FromFile(textBox10.Text);
                int ARGB = 1;
                Invoke(new MethodInvoker(delegate { ARGB = comboBox4.SelectedIndex + 1; }));
                string binary = "";
                int x = 0, y = 0;
                while (binary.Length <= 32)
                {
                    if (x >= img.Width) { x = 0; y++; }
                    Color c = img.GetPixel(x, y);
                    int[] argb = new int[] { c.A, c.R, c.G, c.B };
                    binary += (argb[ARGB] & 1) == 0 ? "1" : "0";
                    x++;
                }

                int size = BitConverter.ToInt32(Program.getByte(binary.Remove(32)), 0) + 1;

                if (size <= 1)
                {
                    MessageBox.Show("No decodable data was detected.", Text,
                        MessageBoxButtons.OK, MessageBoxIcon.Warning); return;
                }

                binary += ProcessBinary(img, 33, (size * 8) + 33, ARGB);

                byte[] data = Program.getByte(binary).Skip(4).ToArray();

                if (textBox9.Text.CompareTo("") != 0)
                {
                    data = Crypto.DecryptStringAES(data, textBox9.Text);
                }

                if (!radioButton7.Checked)
                {
                    Finished f = new Finished(Encoding.ASCII.GetString(data));
                    f.ShowDialog();
                }
                else
                {
                    System.IO.File.WriteAllBytes(textBox7.Text, data);
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
            }
        }

        private string ProcessBinary(Bitmap img, int startPixel, int endPixel, int ARGB)
        {
            int size = endPixel - startPixel;

            List<byte> list = new List<byte>();
            for (int i = startPixel; i <= endPixel; i++)
            {
                Point p = PixelCoord(i, img.Width);
                Color c = img.GetPixel(p.X, p.Y);
                byte[] argb = new byte[] { c.A, c.R, c.G, c.B };
                list.Add(argb[ARGB]);
            }

            return string.Join("",
                Array.ConvertAll<byte, string>(list.ToArray(),
                new Converter<byte, string>(ByteToEvenOdd)));
        }

        private string ByteToEvenOdd(byte b)
        {
            return ((b & 1) == 0) ? "1" : "0";
        }

        #endregion

        private void button5_Click(object sender, EventArgs e)
        {
            textBox2.Text = textBox3.Text = "";
            if (textBox3.Enabled)
            {
                textBox3.Focus();
            }
            else
            {
                textBox2.Focus();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (checkBox4.Checked)
            {
                if (saveFileDialog2.ShowDialog() == DialogResult.OK)
                {
                    textBox7.Text = saveFileDialog2.FileName;
                }
            }
            else
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    textBox7.Text = saveFileDialog1.FileName;
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox10.Text = openFileDialog1.FileName;
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (!checkBox4.Checked)
            {
                tabControl1.Enabled = false;
                Text = "Decoding...";
                backgroundWorker2.RunWorkerAsync();
            }
            else
            {
                tabControl1.Enabled = false;
                Text = "Restoring...";
                backgroundWorker3.RunWorkerAsync();
            }
        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            label10.Enabled = textBox7.Enabled = button6.Enabled = radioButton7.Checked;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (openFileDialog3.ShowDialog() == DialogResult.OK)
            {
                textBox11.Text = openFileDialog3.FileName;
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            label15.Enabled = textBox11.Enabled = button9.Enabled = checkBox4.Checked;
            if (checkBox4.Checked)
            {
                label10.Enabled = textBox7.Enabled = button6.Enabled = true;
                groupBox1.Enabled = false;
                button8.Text = "&Restore";
            }
            else
            {
                label10.Enabled = textBox7.Enabled = button6.Enabled = radioButton5.Checked;
                groupBox1.Enabled = true;
                button8.Text = "&Decode";
            }
        }

        private void backgroundWorker3_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                Bitmap img = (Bitmap)Bitmap.FromFile(textBox10.Text);
                Utilities.IniFile log = new Utilities.IniFile(textBox11.Text);
                int ARGB = log.GetInt32("PROPERTIES", "COLOR", 1);
                int size = log.GetInt32("PROPERTIES", "SIZE", -1);
                if (size != PixelNumber(img.Width, img.Height, img.Width))
                {
                    MessageBox.Show(
                        "The image you have selected is not the same size as the picture that is documented in this log",
                        Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                Dictionary<string, string> pixels = log.GetSectionValues("ORIGINAL");

                foreach (KeyValuePair<string, string> kv in pixels)
                {
                    Point p = PixelCoord(int.Parse(kv.Key) - 1, img.Width);
                    Color c = img.GetPixel(p.X, p.Y);
                    int[] argb = new int[] { c.A, c.R, c.G, c.B };
                    argb[ARGB] = int.Parse(kv.Value);
                    img.SetPixel(p.X, p.Y, Color.FromArgb(argb[0], argb[1], argb[2], argb[3]));
                }
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                img.Dispose();
                Bitmap.FromStream(ms).Save(textBox7.Text, System.Drawing.Imaging.ImageFormat.Png);   
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
