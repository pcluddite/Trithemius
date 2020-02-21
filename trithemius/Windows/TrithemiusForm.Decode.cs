// =====
//
// Copyright (c) 2013-2020 Timothy Baxendale
//
// =====
using System;
using System.ComponentModel;
using System.Diagnostics;
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
        private struct DecodeArgs
        {
            public SteganographyInfo Info { get; }
            public string Password { get; } 
            public string OutputPath { get; }
            public bool IsText => OutputPath == null;
            public bool Legacy { get; }
            public string ImagePath { get; }

            public DecodeArgs(SteganographyInfo info, string imagePath, string pass, string outputPath, bool legacy)
            {
                ImagePath = imagePath;
                Info = info;
                Password = pass;
                OutputPath = outputPath;
                Legacy = legacy;
            }
        }

        private struct DecodeResult
        {
            public DecodeStatus Status { get; }
            public string Message { get; }

            public DecodeResult(DecodeStatus status, string message)
            {
                Status = status;
                Message = message;
            }
        }

        private enum DecodeStatus
        {
            OK, KeyError, GenericError, NoDataError, OptionError
        }

        private void buttonDecode_Click(object sender, EventArgs e)
        {
            string filename = null;
            if (radioButtonFile.Checked) {
                if (msgSaveDialog.ShowDialog(this) != DialogResult.OK) return;
                filename = msgSaveDialog.FileName;
            }
            SteganographyInfo trithemius = CreateTrithemius();
            if (trithemius != null) {
                SetEnabled(false);
                decodeWorker.RunWorkerAsync(new DecodeArgs(trithemius, ImagePath, textBoxKey.Text, filename, checkBoxLegacy.Checked));
            }
        }

        private void decodeWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            DecodeArgs arg = (DecodeArgs)e.Argument;
            try {
                using (Steganographer trithemius = new Steganographer(arg.ImagePath, arg.Info)) {
                    byte[] msg = trithemius.Decode();

                    if (msg == null) {
                        e.Result = new DecodeResult(DecodeStatus.NoDataError, "No decodable data was detected.");
                    }
                    else {
                        if (!string.IsNullOrEmpty(arg.Password)) {
                            if (arg.Legacy) {
                                msg = LegacyEncryption.DecryptStringAES(msg, arg.Password);
                            }
                            else {
                                msg = AESThenHMAC.SimpleDecryptWithPassword(msg, arg.Password);
                            }
                            if (msg == null) {
                                e.Result = new DecodeResult(DecodeStatus.KeyError, "The encryption key was invalid.");
                                return;
                            }
                        }

                        if (arg.OutputPath == null) {
                            e.Result = new DecodeResult(DecodeStatus.OK, Encoding.UTF8.GetString(msg));
                        }
                        else {
                            File.WriteAllBytes(arg.OutputPath, msg);
                            e.Result = new DecodeResult(DecodeStatus.OK, arg.OutputPath);
                        }
                    }
                }
            }
            catch(InvalidImageOptionException ex) {
                e.Result = new DecodeResult(DecodeStatus.OptionError, ex.Message);
            }
            catch (Exception ex) when (ex is EncoderFallbackException || ex is IOException || ex is SecurityException) {
                e.Result = new DecodeResult(DecodeStatus.GenericError, ex.Message);
            }
        }

        private void decodeWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            SetEnabled(true);
            DecodeResult result = (DecodeResult)e.Result;
            if (result.Status == DecodeStatus.OK) {
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
                if (result.Status == DecodeStatus.KeyError) {
                    textBoxKey.SelectAll();
                    textBoxKey.Focus();
                }
            }
        }
    }
}
