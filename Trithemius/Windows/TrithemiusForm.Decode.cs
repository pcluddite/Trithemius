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
using System.Windows.Forms;
using System.Threading.Tasks;
using Monk.Imaging;
using System.ComponentModel;
using Monk.Encryption;
using System.IO;
using System.Security;
using System.Diagnostics;

namespace Trithemius.Windows
{
    partial class TrithemiusForm
    {
        private struct DecodeArgs
        {
            public Steganographer Steganographer { get; }
            public string Password { get; } 
            public string OutputPath { get; }
            public bool IsText => OutputPath == null;
            public bool Legacy { get; }

            public DecodeArgs(Steganographer steganographer, string pass, string outputPath, bool legacy)
            {
                Steganographer = steganographer;
                Password = pass;
                OutputPath = outputPath;
                Legacy = legacy;
            }
        }

        private struct DecodeResult
        {
            public bool Success { get; }
            public string Message { get; }

            public DecodeResult(bool success, string message)
            {
                Success = success;
                Message = message;
            }
        }

        private void buttonDecode_Click(object sender, EventArgs e)
        {
            string filename = null;
            if (radioButtonFile.Checked) {
                if (saveFileDialog.ShowDialog(this) != DialogResult.OK) return;
            }
            Steganographer trithemius = CreateTrithemius();
            SetEnabled(false);
            decodeWorker.RunWorkerAsync(new DecodeArgs(trithemius, textBoxKey.Text, filename, checkBoxLegacy.Checked));
        }

        private void decodeWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            DecodeArgs arg = (DecodeArgs)e.Argument;
            try {
                using (Steganographer trithemius = arg.Steganographer) {
                    byte[] msg = trithemius.Decode();

                    if (msg == null) {
                        e.Result = new DecodeResult(false, "No decodable data was detected.");
                    }

                    if (!string.IsNullOrEmpty(arg.Password)) {
                        if (arg.Legacy) {
                            msg = LegacyEncryption.DecryptStringAES(msg, arg.Password);
                        }
                        else {
                            msg = AESThenHMAC.SimpleDecryptWithPassword(msg, arg.Password);
                        }
                        if (msg == null) {
                            e.Result = new DecodeResult(false, "The encryption key was invalid.");
                            return;
                        }
                    }

                    if (arg.OutputPath == null) {
                        e.Result = new DecodeResult(true, Encoding.UTF8.GetString(msg));
                    }
                    else {
                        File.WriteAllBytes(arg.OutputPath, msg);
                        e.Result = new DecodeResult(true, arg.OutputPath);
                    }
                }
            }
            catch (Exception ex) when (ex is ArgumentException || ex is EncoderFallbackException || ex is IOException || ex is SecurityException || ex is InvalidOperationException) {
                e.Result = new DecodeResult(false, ex.Message);
            }
        }

        private void decodeWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            SetEnabled(true);
            DecodeResult result = (DecodeResult)e.Result;
            if (result.Success) {
                if (radioButtonFile.Checked) {
                    if (MessageBox.Show(this, "Decoding complete. Would you like to open the file?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                        TryStart(new ProcessStartInfo(result.Message));
                    }
                }
                else {
                    Finished finished = new Finished(result.Message);
                    finished.ShowDialog(this);
                }
            }
            else {
                ShowError(result.Message);
            }
        }
    }
}
