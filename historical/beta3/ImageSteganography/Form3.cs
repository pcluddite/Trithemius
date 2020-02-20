using System;
using System.Windows.Forms;

namespace Trithemius
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Hide();
            Form1 f = new Form1();
            f.ShowDialog();
            Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Hide();
            Form2 f = new Form2();
            f.ShowDialog();
            Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Hide();
            Form5 f = new Form5();
            f.ShowDialog();
            Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Hide();
            Form7 f = new Form7();
            f.ShowDialog();
            Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Hide();
            Form8 f = new Form8();
            f.ShowDialog();
            Show();
        }

        private void button1_MouseHover(object sender, EventArgs e) {
            toolTip1.ToolTipTitle = "Encoding";
            toolTip1.ToolTipIcon = ToolTipIcon.Info;
            toolTip1.Show("Encode hidden messages or files into images.", button1);
        }

        private void button2_MouseHover(object sender, EventArgs e) {
            toolTip1.ToolTipTitle = "Decoding";
            toolTip1.ToolTipIcon = ToolTipIcon.Info;
            toolTip1.Show("Decode hidden messages or files\r\npreviously encoded in an image.", button2);
        }

        private void button7_MouseHover(object sender, EventArgs e) {
            toolTip1.ToolTipTitle = "Text Encoding";
            toolTip1.ToolTipIcon = ToolTipIcon.Info;
            toolTip1.Show("Use this utility to convert serveral text encodings\r\ninto decimal, hexadecimal, or binary.", button7);
        }

        private void button4_MouseHover(object sender, EventArgs e) {
            toolTip1.ToolTipTitle = "Encryption";
            toolTip1.ToolTipIcon = ToolTipIcon.Info;
            toolTip1.Show("Utilize this early method of encryption.", button4);
        }
    }
}
