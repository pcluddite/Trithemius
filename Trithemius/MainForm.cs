using System;
using System.Windows.Forms;

namespace Trithemius
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Hide();
            EncodeForm f = new EncodeForm();
            f.ShowDialog();
            Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Hide();
            DecodeForm f = new DecodeForm();
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
            toolTip1.Show("Decode hidden messages or files previously encoded in an image.", button2);
        }
    }
}
