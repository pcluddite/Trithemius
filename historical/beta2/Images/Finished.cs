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
    public partial class Finished : Form
    {
        public Finished(string text)
        {
            InitializeComponent();
            textBox2.Text = text;
        }
    }
}
