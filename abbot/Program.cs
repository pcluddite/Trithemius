﻿/**
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
using System.IO;
using System.Text;

namespace Abbot
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var results = Parser.Default.ParseArguments<EncodeOptions, DecodeOptions>(args)
                .MapResult(
                    (EncodeOptions opts) => EncodeImage(opts),
                    (DecodeOptions opts) => DecodeImage(opts),
                    errors => GetError(errors)
                );
            Console.ReadKey(true);
        }

        private static int EncodeImage(EncodeOptions opts)
        {
            try {
                Steganographer trithemius = opts.BuildTrithemius();
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

                var changes = trithemius.Encode(data, opts.Output);
                if (opts.Verbose) {
                    foreach (var diff in changes) {
                        Console.WriteLine("{0} => {1}", diff.Key, diff.Value);
                    }
                }
                
                return 0;
            }
            catch(Exception ex) when (ex is ArgumentException || ex is IOException) {
                Console.Error.WriteLine(ex.Message);
                return 1;
            }
        }

        private static int DecodeImage(DecodeOptions opts)
        {
            Steganographer trithemius = opts.BuildTrithemius();
            return 0;
        }

        private static int GetError(IEnumerable<Error> errors)
        {
            return 1;
        }
    }
}