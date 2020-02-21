// =====
//
// Copyright (c) 2013-2020 Timothy Baxendale
//
// =====
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
            inputTextForm = new Text();
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

        private SteganographyInfo CreateTrithemius()
        {
            try {
                SteganographyInfo trithemius;

                if (checkBoxLegacy.Checked) {
                    trithemius = SteganographyInfo.LegacyOptions;
                }
                else {
                    trithemius = new SteganographyInfo()
                    {
                        Offset = (int)numericUpDownOffset.Value - 1,
                        LeastSignificantBits = (int)numericUpDownLsb.Value,
                        InvertDataBits = checkBoxInvertData.Checked,
                        InvertPrefixBits = checkBoxInvertPrefix.Checked,
                        Endianness = comboBoxEndian.SelectedIndex == 0 ? EndianMode.LittleEndian : EndianMode.BigEndian
                    };
                }

                if (!string.IsNullOrEmpty(textBoxSeed.Text)) {
                    string seedtext = textBoxSeed.Text;
                    ushort[] digits = new ushort[seedtext.Length];
                    for (int idx = 0; idx < seedtext.Length; ++idx) {
                        char c = seedtext[idx];
                        if (char.IsDigit(c)) {
                            digits[idx] = (ushort)(c - '0');
                        }
                        else {
                            textBoxSeed.Select(idx, 1);
                            textBoxSeed.Focus();
                            throw new ArgumentException("All characters in a seed must be a number");
                        }
                    }
                    trithemius.Seed = digits;
                }

                if (checkAlpha.Checked && checkAlpha.Enabled) trithemius.Colors.Add(PixelColor.Alpha);
                if (checkRed.Checked && checkRed.Enabled) trithemius.Colors.Add(PixelColor.Red);
                if (checkGreen.Checked && checkGreen.Enabled) trithemius.Colors.Add(PixelColor.Green);
                if (checkBlue.Checked && checkBlue.Enabled) trithemius.Colors.Add(PixelColor.Blue);
                return trithemius;
            }
            catch (ArgumentException ex) {
                ShowError(ex.Message);
                return null;
            }
        }
    }
}
