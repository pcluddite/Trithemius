﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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
    }
}
