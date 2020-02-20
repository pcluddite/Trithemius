using System;
using System.Windows.Forms;

namespace Trithemius
{
    public partial class Text : Form
    {
        Form1 form;
        EncoderNew form2;
        bool formNew = false;
        public Text(Form1 form)
        {
            this.form = form;
            InitializeComponent();
            textBox1.Text = form.message;
        }

        public Text(EncoderNew form)
        {
            formNew = true;
            this.form2 = form;
            InitializeComponent();
            textBox1.Text = form2.message;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (formNew)
                form2.message = textBox1.Text;
            else
                form.message = textBox1.Text;
            Close();
        }
    }
}
