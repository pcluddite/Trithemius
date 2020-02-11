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
