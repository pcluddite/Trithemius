using System;
using System.Windows.Forms;

namespace Trithemius
{
    public partial class Text : Form
    {
        EncodeForm form;

        public Text(EncodeForm form)
        {
            this.form = form;
            InitializeComponent();
            textBox1.Text = form.message;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            form.message = textBox1.Text;
            Close();
        }
    }
}
