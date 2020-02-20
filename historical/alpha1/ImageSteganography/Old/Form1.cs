using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace ImageSteganography
{
    public partial class Form1 : Form
    {
        public Form1()
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

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                button1.Enabled = button2.Enabled = button3.Enabled =
                    button4.Enabled = textBox1.Enabled = textBox3.Enabled =
                    label1.Enabled = label2.Enabled = label3.Enabled = 
                    radioButton1.Enabled = radioButton2.Enabled =
                    comboBox1.Enabled = false;
                Text = "Decoding in progress...";
                backgroundWorker1.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            label2.Enabled = textBox3.Enabled = button4.Enabled = radioButton2.Checked;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox3.Text = saveFileDialog1.FileName;
            }
        }

        int ARGB = 1;
        Bitmap img = null;

        private int PixelNumber(int x, int y, int width)
        {
            return (y * width) + x;
        }

        private Point PixelCoord(int number, int width)
        {
            return new Point(number % width, (number - (number % width)) / width);
        }

        private string EvenOddBinary(byte[] bytes)
        {
            return string.Join("",
                Array.ConvertAll<byte, string>(bytes,
                new Converter<byte, string>(ByteToEvenOdd)));
        }

        private string ByteToEvenOdd(byte b)
        {
            return ((b & 1) == 0) ? "1" : "0";
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                Invoke(new MethodInvoker(delegate { img = (Bitmap)Bitmap.FromFile(textBox1.Text); }));
                Invoke(new MethodInvoker(delegate { ARGB = comboBox1.SelectedIndex + 1; }));
                string binary = "";
                int x = 0, y = 0;
                while (binary.Length <= 32)
                {
                    if (x >= img.Width) { x = 0; y++; }
                    Color c = img.GetPixel(x, y);
                    switch (ARGB)
                    {
                        case 0:
                            binary += (c.A & 1) == 0 ? "1" : "0"; break;
                        case 1:
                            binary += (c.R & 1) == 0 ? "1" : "0"; break;
                        case 2:
                            binary += (c.G & 1) == 0 ? "1" : "0"; break;
                        case 3:
                            binary += (c.B & 1) == 0 ? "1" : "0"; break;
                    }
                    x++;
                }

                int size = BitConverter.ToInt32(Program.getByte(binary.Remove(32)), 0) + 1;

                if (size <= 0) { 
                    MessageBox.Show("No decodable data was detected.", Text, 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

                binary += ProcessBinary(33, (size * 8) + 33, ARGB);

                byte[] data = Program.getByte(binary).Skip(4).ToArray();

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
                MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            button1.Enabled = button2.Enabled = button3.Enabled =
                textBox1.Enabled =  comboBox1.Enabled =
                radioButton1.Enabled = radioButton2.Enabled=
                label1.Enabled = label3.Enabled = true;
            label2.Enabled = textBox3.Enabled = button4.Enabled = radioButton2.Checked;
           
            Text = "Decoder";
            if (radioButton2.Checked &&
                MessageBox.Show(this, "Decoding Completed!\r\nWould you like to open the file?",
                    Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                System.Diagnostics.Process.Start(textBox3.Text);
            }
        }

        private string ProcessBinary(int startPixel, int endPixel, int ARGB)
        {
            string binary = "";
            int size = endPixel - startPixel;

            List<byte> c = new List<byte>();
            for (int i = startPixel; i <= endPixel; i++)
            {
                Point p = PixelCoord(i, img.Width);
                switch (ARGB)
                {
                    case 0: c.Add(img.GetPixel(p.X, p.Y).A); break;
                    case 1: c.Add(img.GetPixel(p.X, p.Y).R); break;
                    case 2: c.Add(img.GetPixel(p.X, p.Y).G); break;
                    case 3: c.Add(img.GetPixel(p.X, p.Y).B); break;
                }
            }
            binary = EvenOddBinary(c.ToArray());

            return binary;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {    
                img = (Bitmap)Bitmap.FromFile(textBox1.Text);
                string binary = ProcessBinary(0, 32, comboBox1.SelectedIndex + 1);
                int size = (BitConverter.ToInt32(
                Program.getByte(binary), 0) * 8);
                if (size <= 0) { MessageBox.Show("No decodable data was detected.", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                MessageBox.Show(this, size + 8 +
                " Bits detected", Text,
                MessageBoxButtons.OK, MessageBoxIcon.Information);
                img.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
