/**
 *  Abbot
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
using Abbot.Arguments;
using CommandLine;
using Monk.Encryption;
using Monk.Imaging;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace Abbot
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            int exitCode = Parser.Default.ParseArguments<EncodeOptions, DecodeOptions>(args)
                .MapResult(
                    (EncodeOptions opts) => EncodeImage(opts),
                    (DecodeOptions opts) => DecodeImage(opts),
                    errors => GetError(errors)
                );
            Environment.Exit(exitCode);
        }

        private static int EncodeImage(EncodeOptions opts)
        {
            Steganographer trithemius = null;
            try {
                trithemius = opts.BuildTrithemius();
                byte[] data;
                if (!string.IsNullOrEmpty(opts.Message)) {
                    data = Encoding.UTF8.GetBytes(opts.Message);
                }
                else if (!string.IsNullOrEmpty(opts.File)) {
                    data = File.ReadAllBytes(opts.File);
                }
                else {
                    throw new ArgumentException("No data was specified to encode. Please specify either --message or --file to encode data");
                }

                if (!string.IsNullOrEmpty(opts.Key)) {
                    data = AESThenHMAC.SimpleEncryptWithPassword(data, opts.Key);
                }

                trithemius.Encode(data);

                trithemius.Image.Save(opts.Output, ImageFormat.Png);

                return 0;
            }
            catch (Exception ex) when (ex is ArgumentException || ex is IOException) {
                Console.Error.WriteLine(ex.Message);
                return 1;
            }
            finally {
                if (trithemius != null) trithemius.Dispose();
            }
        }

        private static int DecodeImage(DecodeOptions opts)
        {
            Steganographer trithemius = null;
            try {
                trithemius = opts.BuildTrithemius();
                byte[] data = trithemius.Decode();
                if (!string.IsNullOrEmpty(opts.Key)) {
                    if (opts.Legacy) {
                        data = LegacyEncryption.DecryptStringAES(data, opts.Key);
                    }
                    else {
                        data = AESThenHMAC.SimpleDecryptWithPassword(data, opts.Key);
                    }
                    if (data == null) {
                        throw new ArgumentException("The encryption key is incorrect");
                    }
                }

                if (string.IsNullOrEmpty(opts.Output)) {
                    Console.WriteLine(Encoding.UTF8.GetString(data));
                }
                else {
                    File.WriteAllBytes(opts.Output, data);
                }

                return 0;
            }
            catch (Exception ex) when (ex is ArgumentException || ex is IOException) {
                Console.Error.WriteLine(ex.Message);
                return 1;
            }
            finally {
                if (trithemius != null) trithemius.Dispose();
            }
        }

        private static int GetError(IEnumerable<Error> errors)
        {
            return 1;
        }
    }
}
