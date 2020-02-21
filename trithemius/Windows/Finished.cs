// =====
//
// Copyright (c) 2013-2020 Timothy Baxendale
//
// =====
using System;
using System.IO;
using System.Windows.Forms;
using System.Security;

namespace Trithemius.Windows
{
    public partial class Finished : Form
    {
        public Finished(string text)
        {
            InitializeComponent();
            txtOutput.Text = text;
            txtOutput.Select(0, 0);
        }

        private void closeButton_Click(object sender, System.EventArgs e)
        {
            Close();
        }

        private void saveButton_Click(object sender, System.EventArgs e)
        {
            if (saveFileDialog.ShowDialog() == DialogResult.OK) {
                try {
                    File.WriteAllText(saveFileDialog.FileName, txtOutput.Text);
                }
                catch(Exception ex) when (ex is IOException || ex is SecurityException) {
                    MessageBox.Show(this, ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
