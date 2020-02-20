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
    public partial class Form7 : Form
    {
        public Form7()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK &&
                saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                button1.Enabled = button2.Enabled = checkBox1.Enabled = false;
                backgroundWorker1.RunWorkerAsync();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK &&
                saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                button1.Enabled = button2.Enabled = checkBox1.Enabled = false;
                backgroundWorker2.RunWorkerAsync();
            }
        }

        delegate void progressChanged(int p);

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                Delegate d = new progressChanged(backgroundWorker1_ProgressChanged);
                byte[] file = System.IO.File.ReadAllBytes(openFileDialog1.FileName);
                List<byte> data = new List<byte>();
                data.AddRange(BitConverter.GetBytes(file.Length));
                data.AddRange(file);

                Color[] colors = new Color[data.Count];
                int rgb = 0;
                int i = -1;
                int progress = 0;
                foreach (byte b in data)
                {
                    if ((rgb % 3) == 0) { i++; }
                    byte[] COLOR = new byte[] { colors[i].R, colors[i].G, colors[i].B };
                    COLOR[rgb % 3] = b;
                    colors[i] = Color.FromArgb(COLOR[0], COLOR[1], COLOR[2]);
                    rgb++;
                    progress = (int)Math.Truncate(100 * ((decimal)rgb / (decimal)data.Count));
                    //Invoke(d, progress);
                }

                colors = colors.Take(i).ToArray();

                int width = (int)Math.Truncate(Math.Sqrt(colors.Length)) + 1;
                int height = PixelCoord(colors.Length, width).Y + 1;
                Bitmap img = new Bitmap(width, height);
                for (i = 0; i < colors.Length; i++)
                {
                    Point p = PixelCoord(i, width);
                    img.SetPixel(p.X, p.Y, colors[i]);
                }
                img.Save(saveFileDialog1.FileName, System.Drawing.Imaging.ImageFormat.Png);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                Bitmap img = (Bitmap)Image.FromFile(openFileDialog1.FileName);
                List<byte> data = new List<byte>();
                for (int i = 0; i < PixelNumber(img.Width - 1, img.Height - 1, img.Width); i++)
                {
                    Point p = PixelCoord(i, img.Width);
                    Color c = img.GetPixel(p.X, p.Y);
                    data.Add(c.R);
                    data.Add(c.G);
                    data.Add(c.B);
                }
                int size = BitConverter.ToInt32(data.Take(4).ToArray(), 0);
                System.IO.File.WriteAllBytes(saveFileDialog1.FileName,
                    data.Skip(4).Take(size).ToArray());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void backgroundWorker1_ProgressChanged(int prog)
        {
            progressBar1.Value = prog;
            label1.Text = prog+ "%";
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            button1.Enabled = button2.Enabled = checkBox1.Enabled = true;
        }
    }
}
