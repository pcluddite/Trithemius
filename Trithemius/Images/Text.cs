using System;
using System.Windows.Forms;

namespace Trithemius
{
    public partial class Text : Form
    {

        public string Result { get; private set; }

        public Text(string msg)
        {
            InitializeComponent();
            textBox1.Text = msg;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Result = textBox1.Text;
            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
