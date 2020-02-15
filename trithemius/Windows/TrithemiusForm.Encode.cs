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
using System.IO;
using System.Security;
using System.Text;
using System.Windows.Forms;

using Monk.Encryption;
using Monk.Imaging;

namespace Trithemius.Windows
{
    partial class TrithemiusForm
    {
        private struct EncodeArgs
        {
            public Steganographer Steganographer { get; }
            public string Password { get; }
            public string Data { get; }
            public bool IsText { get; }
            public string OutputPath { get; }

            public EncodeArgs(Steganographer steganographer, string data, bool isText, string pass, string outputPath)
            {
                Steganographer = steganographer;
                Password = pass;
                Data = data;
                IsText = isText;
                OutputPath = outputPath;
            }
        }

        private struct EncodeResult
        {
            public bool Success { get; }
            public string Message { get; }

            public EncodeResult(bool success, string message)
            {
                Success = success;
                Message = message;
            }
        }

        private void buttonEncode_Click(object sender, EventArgs e)
        {
            string message = null;
            if (radioButtonFile.Checked) {
                if (msgOpenDialog.ShowDialog(this) == DialogResult.OK) {
                    message = msgOpenDialog.FileName;
                }
            }
            else {
                if (inputTextForm.ShowDialog(this) == DialogResult.OK) {
                    message = inputTextForm.Result;
                }
            }
            if (message != null && saveFileDialog.ShowDialog(this) == DialogResult.OK) {
                Steganographer trithemius = CreateTrithemius();
                if (trithemius != null) {
                    SetEnabled(false);
                    encodeWorker.RunWorkerAsync(new EncodeArgs(trithemius, message, radioButtonText.Checked, textBoxKey.Text, saveFileDialog.FileName));
                }
            }
        }

        private void encodeWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try {
                EncodeArgs args = (EncodeArgs)e.Argument;
                using (Steganographer trithemius = args.Steganographer) {
                    byte[] data;
                    if (args.IsText) {
                        data = Encoding.UTF8.GetBytes(args.Data);
                    }
                    else {
                        data = File.ReadAllBytes(args.Data);
                    }
                    if (!string.IsNullOrEmpty(args.Password)) {
                        data = AESThenHMAC.SimpleEncryptWithPassword(data, args.Password);
                    }
                    trithemius.Encode(data);
                    trithemius.SaveImage(args.OutputPath);
                    e.Result = new EncodeResult(true, args.OutputPath);
                }
            }
            catch(Exception ex) when (ex is EncoderFallbackException || ex is IOException || ex is SecurityException || ex is InvalidOperationException) {
                e.Result = new EncodeResult(false, ex.Message);
            }
        }

        private void encodeWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            SetEnabled(true);
            EncodeResult result = (EncodeResult)e.Result;
            if (result.Success) {
                ShowInfo("Encoding completed! You should spot no visible image differences.");
                if (result.Message == ImagePath) {
                    ReloadPreview(ImagePath);
                }
            }
            else { 
                ShowError(result.Message);
            }
        }
    }
}
