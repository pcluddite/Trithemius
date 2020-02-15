// =====
//
// Copyright (c) 2013-2020 Timothy Baxendale
//
// =====
using System;
using System.Windows.Forms;

namespace Trithemius.Windows
{
    public partial class RandomSeed : Form
    {
        public int Result { get; private set; }

        public RandomSeed(decimal value)
        {
            InitializeComponent();
            seedNumericUpDown.Value = value;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void generateButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Result = (int)seedNumericUpDown.Value;
            Close();
        }
    }
}
