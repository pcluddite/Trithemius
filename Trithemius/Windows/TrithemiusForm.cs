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
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

using Monk.Imaging;
using Monk.Memory;
using Monk.Memory.Bittwiddling;

namespace Trithemius.Windows
{
    public partial class TrithemiusForm : Form
    {
        private readonly Text inputTextForm;

        public TrithemiusForm()
        {
            InitializeComponent();
            inputTextForm = new Text("");
        }

        private void TrithemiusForm_Load(object sender, EventArgs e)
        {
            comboBoxEndian.SelectedIndex = 0;
        }

        private void SetEnabled(bool enabled)
        {
            buttonEncode.Enabled = buttonDecode.Enabled = groupBoxPath.Enabled = 
                groupBoxImage.Enabled = groupBoxEncode.Enabled = enabled;
        }

        private Process TryStart(ProcessStartInfo startInfo, bool displayErrors = true)
        {
            try {
                return Process.Start(startInfo);
            }
            catch (Exception ex) when (ex is InvalidOperationException || ex is IOException || ex is Win32Exception) {
                if (displayErrors) ShowError(ex.Message);
                return null;
            }
        }

        private static Image LoadImage(string filename)
        {
            Stream stream = new MemoryStream(File.ReadAllBytes(filename));
            return Bitmap.FromStream(stream);
        }

        private static Image CopyImage(Image image)
        {
            Stream stream = new MemoryStream();
            image.Save(stream, image.RawFormat);
            return Bitmap.FromStream(stream);
        }

        private void ShowError(string message)
        {
            MessageBox.Show(this, message, Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void ShowInfo(string message)
        {
            MessageBox.Show(this, message, Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private Steganographer CreateTrithemius()
        {
            Steganographer trithemius = new Steganographer((Bitmap)CopyImage(pictureBox.Image))
            {
                Offset = (int)numericUpDownOffset.Value - 1,
                LeastSignificantBits = (int)numericUpDownLsb.Value,
                InvertDataBits = checkBoxInvertData.Checked,
                InvertPrefixBits = checkBoxInvertPrefix.Checked,
                Endianness = comboBoxEndian.SelectedIndex == 0 ? EndianMode.LittleEndian : EndianMode.BigEndian
            };

            if (!string.IsNullOrEmpty(textBoxSeed.Text)) {
                trithemius.Seed = new Seed(textBoxSeed.Text);
            }

            if (checkBoxLegacy.Checked) {
                trithemius.SetLegacyOptions();
            }

            if (checkAlpha.Checked && checkAlpha.Enabled) trithemius.Colors.Add(PixelColor.Alpha);
            if (checkRed.Checked && checkRed.Enabled) trithemius.Colors.Add(PixelColor.Red);
            if (checkGreen.Checked && checkGreen.Enabled) trithemius.Colors.Add(PixelColor.Green);
            if (checkBlue.Checked && checkBlue.Enabled) trithemius.Colors.Add(PixelColor.Blue);

            return trithemius;
        }
    }
}
