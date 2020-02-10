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
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Monk.Imaging;
using Monk.Memory;
using Monk.Memory.Bittwiddling;

namespace Trithemius.Windows
{
    partial class TrithemiusForm
    { 
        private void RefreshOptions()
        {
            Image image = pictureBox.Image;
            if (image == null || "Refresh" == buttonBrowse.Text) {
                groupBoxEncode.Enabled = false;
                buttonEncode.Enabled = false;
                buttonDecode.Enabled = false;
            }
            else {
                buttonEncode.Enabled = true;
                buttonDecode.Enabled = true;
                groupBoxEncode.Enabled = true;
                RefreshColorOptions(image.PixelFormat);
            }
        }

        private void numericUpDownLsb_ValueChanged(object sender, EventArgs e)
        {
            ReloadMaxSize(pictureBox.Image);
        }

        private void RefreshColorOptions(PixelFormat format)
        {
            switch (format) {
                case PixelFormat.Format32bppArgb: 
                    checkAlpha.Enabled = true;
                    checkRed.Enabled = true;
                    checkGreen.Enabled = true;
                    checkBlue.Enabled = true;
                    break;
                case PixelFormat.Format24bppRgb:
                    checkAlpha.Enabled = false;
                    checkRed.Enabled = true;
                    checkGreen.Enabled = true;
                    checkBlue.Enabled = true;
                    break;
                case PixelFormat.Format8bppIndexed:
                    checkAlpha.Enabled = false;
                    checkRed.Enabled = false;
                    checkGreen.Enabled = false;
                    checkBlue.Enabled = true;
                    break;
                default:
                    checkAlpha.Enabled = false;
                    checkRed.Enabled = false;
                    checkGreen.Enabled = false;
                    checkBlue.Enabled = false;
                    break;
            }
        }

        private void checkBoxLegacy_CheckedChanged(object sender, EventArgs e)
        {
            buttonEncode.Enabled = groupBoxStartPixel.Enabled = groupBoxPrefix.Enabled = 
                groupBoxInvert.Enabled = groupBoxEndian.Enabled = groupBoxLsb.Enabled = 
                checkAlpha.Enabled = !checkBoxLegacy.Checked;
            if (checkRed.Checked) {
                checkAlpha.Checked = checkGreen.Checked = checkBlue.Checked = false;
            }
            else if (checkGreen.Checked) {
                checkAlpha.Checked = checkRed.Checked = checkBlue.Checked = false;
            }
            else if (checkBlue.Checked) {
                checkAlpha.Checked = checkRed.Checked = checkGreen.Checked = false;
            }
        }

        private void checkAlpha_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxLegacy.Checked && checkAlpha.Checked) {
                checkRed.Checked = checkGreen.Checked = checkBlue.Checked = false;
            }
        }

        private void checkRed_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxLegacy.Checked && checkRed.Checked) {
                checkAlpha.Checked = checkGreen.Checked = checkBlue.Checked = false;
            }
        }

        private void checkGreen_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxLegacy.Checked && checkGreen.Checked) {
                checkAlpha.Checked = checkRed.Checked = checkBlue.Checked = false;
            }
        }

        private void checkBlue_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxLegacy.Checked && checkBlue.Checked) {
                checkAlpha.Checked = checkRed.Checked = checkGreen.Checked = false;
            }
        }
    }
}
