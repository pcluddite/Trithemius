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
    public partial class Text : Form
    {
        Form1 form;
        public Text(Form1 form)
        {
            this.form = form;
            InitializeComponent();
            textBox1.Text = form.message;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            form.message = textBox1.Text;
            Close();
        }
    }
}
