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
    public partial class Encrypt : Form
    {
        public Encrypt()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            label7.Enabled = textBox2.Enabled = radioButton1.Checked;
            label2.Enabled = textBox3.Enabled = button4.Enabled = radioButton2.Checked;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox3.Text = openFileDialog1.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                label1.Enabled = label2.Enabled = label3.Enabled =
                    label7.Enabled = label8.Enabled = textBox1.Enabled =
                    textBox2.Enabled = textBox3.Enabled = textBox4.Enabled =
                    button1.Enabled = button2.Enabled = button3.Enabled =
                    button4.Enabled = comboBox1.Enabled = radioButton1.Enabled =
                    radioButton2.Enabled = false;
                Text = "Encoding...";
                backgroundWorker1.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        string boolToString(bool b)
        {
            return b ? "1" : "0";
        }

        private int PixelNumber(int x, int y, int width)
        {
            return (y * width) + x;
        }

        private Point PixelCoord(int number, int width)
        {
            return new Point(number % width, (number - (number % width)) / width);
        }


        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                Bitmap img = (Bitmap)Bitmap.FromFile(textBox1.Text);
                int ARGB = 1;

                byte[] msg = radioButton1.Checked ? 
                    Encoding.ASCII.GetBytes(textBox2.Text) :
                    System.IO.File.ReadAllBytes(textBox3.Text);
                
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
                foreach (char c in binary)
                {
                    int argb = allPixels[i];

                    b = c == '1' ?
                        (argb & 1) != 0 ?
                        argb == 255 ? 254 : argb + 1 : argb
                        : (argb & 1) == 0 ? argb + 1 : argb;
                    nData.Add((byte)b);
                    i++;
                }

                img = setPixels(img, nData.ToArray(), ARGB);

                img.Save(textBox4.Text, System.Drawing.Imaging.ImageFormat.Png);
                img.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        byte[] getPixels(Bitmap img, int size, int ARGB)
        {
            List<byte> data = new List<byte>();
            for (int i = 0; i <= size; i++)
            {
                Point p = PixelCoord(i, img.Width);
                switch (ARGB)
                {
                    case 0: data.Add(img.GetPixel(p.X, p.Y).A); break;
                    case 1: data.Add(img.GetPixel(p.X, p.Y).R); break;
                    case 2: data.Add(img.GetPixel(p.X, p.Y).G); break;
                    case 3: data.Add(img.GetPixel(p.X, p.Y).B); break;
                }
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
                switch (ARGB)
                {
                    case 0:
                        img.SetPixel(p.X, p.Y, Color.FromArgb(b, col.R, col.G, col.B));
                        break;
                    case 1:
                        img.SetPixel(p.X, p.Y, Color.FromArgb(col.A, b, col.G, col.B));
                        break;
                    case 2:
                        img.SetPixel(p.X, p.Y, Color.FromArgb(col.A, col.R, b, col.B));
                        break;
                    case 3:
                        img.SetPixel(p.X, p.Y, Color.FromArgb(col.A, col.R, col.G, b));
                        break;
                }
                i++;
            }
            return img;
        }

        private void Encrypt_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox4.Text = saveFileDialog1.FileName;
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            label1.Enabled = label3.Enabled = label8.Enabled = 
                textBox1.Enabled = textBox4.Enabled = button1.Enabled = 
                button2.Enabled = button3.Enabled = comboBox1.Enabled = 
                radioButton1.Enabled = radioButton2.Enabled = true;
            label7.Enabled = textBox2.Enabled = radioButton1.Checked;
            label2.Enabled = textBox3.Enabled = button4.Enabled = radioButton2.Checked;
            Text = "Steganographer";
            MessageBox.Show(this, "Encoding completed! You should spot no visible image differences.", Text, 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
