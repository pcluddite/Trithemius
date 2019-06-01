/**
 *  Trithemius
 *  Copyright (C) Timothy Baxendale
 *
 *  This program is free software; you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation; either version 2 of the License, or
 *  (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License along
 *  with this program; if not, write to the Free Software Foundation, Inc.,
 *  51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA.
**/
using System;
using System.IO;
using System.Windows.Forms;
using System.Security;

namespace Trithemius
{
    public partial class Finished : Form
    {
        public Finished(string text)
        {
            InitializeComponent();
            textBox2.Text = text;
        }

        private void closeButton_Click(object sender, System.EventArgs e)
        {
            Close();
        }

        private void saveButton_Click(object sender, System.EventArgs e)
        {
            if (saveFileDialog.ShowDialog() == DialogResult.OK) {
                try {
                    File.WriteAllText(saveFileDialog.FileName, textBox2.Text);
                }
                catch(Exception ex) when (ex is IOException || ex is SecurityException) {
                    Program.ShowError(this, ex.Message);
                }
            }
        }
    }
}
