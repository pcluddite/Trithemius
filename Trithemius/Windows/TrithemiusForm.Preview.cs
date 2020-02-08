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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;

namespace Trithemius.Windows
{
    partial class TrithemiusForm
    {
        public string ImagePath { get; set; }

        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            if ("Browse" == ((Button)sender).Text) {
                string path = Browse();
                if (path != null) {
                    ImagePath = path;
                    pathTextBox.Text = openFileDialog.FileName;
                }
            }
            try {
                ReloadPreview(pathTextBox.Text);
                ImagePath = pathTextBox.Text;
                ((Button)sender).Text = "Browse";
                RefreshOptions();
            }
            catch(IOException ex) {
                ShowError(ex.Message);
            }
        }

        private string Browse()
        {
            if (!string.IsNullOrWhiteSpace(pathTextBox.Text) && pathTextBox.Text != openFileDialog.FileName) {
                try {
                    openFileDialog.FileName = Path.GetFileName(pathTextBox.Text);
                    openFileDialog.InitialDirectory = Path.GetDirectoryName(pathTextBox.Text);
                }
                catch (Exception ex) when (ex is PathTooLongException || ex is ArgumentException) {
                    // ignore
                }
            }

            if (openFileDialog.ShowDialog(this) == DialogResult.OK) {
                return openFileDialog.FileName;
            }
            else {
                return null;
            }
        }

        private void pathTextBox_TextChanged(object sender, EventArgs e)
        {
            buttonBrowse.Text = pathTextBox.Text == ImagePath ? "Browse" : "Refresh";
            RefreshOptions();
        }

        private void ReloadPreview(string path)
        {
            Image image = LoadImage(path);
            ReloadDetails(image);
            pictureBox.Image = image;
            RefreshOptions();
        }

        private void ReloadDetails(Image image)
        {
            int area = image.Width * image.Height;
            string format = image.PixelFormat.ToString();
            if (format.StartsWith("Format")) format = format.Substring("Format".Length);

            textBoxFormat.Text = format;
            textBoxWidth.Text = $"{image.Width} px";
            textBoxHeight.Text = $"{image.Height} px";
            ReloadMaxSize(image);
            numericUpDownOffset.Value = Math.Min(numericUpDownOffset.Value, area);
            numericUpDownOffset.Maximum = area;
        }

        private void ReloadMaxSize(Image image)
        {
            int area = image.Width * image.Height;
            textBoxMaxSize.Text = (((Image.GetPixelFormatSize(image.PixelFormat) / 8) * area) * numericUpDownLsb.Value).ToString("#,##0");
        }

        private static Image LoadImage(string filename)
        {
            MemoryStream stream = new MemoryStream(File.ReadAllBytes(filename));
            return Image.FromStream(stream);
        }
    }
}
