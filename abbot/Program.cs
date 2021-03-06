﻿// =====
//
// Copyright (c) 2013-2020 Timothy Baxendale
//
// =====
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Abbot.Arguments;

using CommandLine;

using Monk.Encryption;
using Monk.Imaging;

namespace Abbot
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            int exitCode = Parser.Default.ParseArguments<EncodeOptions, DecodeOptions>(args)
                                 .MapResult(
                                     (EncodeOptions opts) => EncodeImage(opts),
                                     (DecodeOptions opts) => DecodeImage(opts),
                                     GetError
                                 );
            Environment.Exit(exitCode);
        }

        private static int EncodeImage(EncodeOptions opts)
        {
            try {
                byte[] data;

                if (!string.IsNullOrEmpty(opts.Message)) {
                    data = Encoding.UTF8.GetBytes(opts.Message);
                } else if (!string.IsNullOrEmpty(opts.File)) {
                    data = File.ReadAllBytes(opts.File);
                } else {
                    throw new ArgumentException("No data was specified to encode. Please specify either --message or --file to encode data");
                }

                if (!string.IsNullOrEmpty(opts.Key)) {
                    data = AESThenHMAC.SimpleEncryptWithPassword(data, opts.Key);
                }

                using (Steganographer trithemius = new Steganographer(opts.Path, opts.BuildTrithemius())) {
                    trithemius.Encode(data);
                    trithemius.SaveImage(opts.Output);
                }

                return 0;
            }
            catch (Exception ex) when (ex is ArgumentException || ex is IOException) {
                Console.Error.WriteLine(ex.Message);
                return 1;
            }
        }

        private static int DecodeImage(DecodeOptions opts)
        {
            try {
                using (Steganographer trithemius = new Steganographer(opts.Path, opts.BuildTrithemius())) {
                    byte[] data = trithemius.Decode();

                    if (!string.IsNullOrEmpty(opts.Key)) {
                        if (opts.Legacy) {
                            data = LegacyEncryption.DecryptStringAES(data, opts.Key);
                        } else {
                            data = AESThenHMAC.SimpleDecryptWithPassword(data, opts.Key);
                        }

                        if (data == null) {
                            throw new ArgumentException("The encryption key is incorrect");
                        }
                    }

                    if (string.IsNullOrEmpty(opts.Output)) {
                        Console.WriteLine(Encoding.UTF8.GetString(data));
                    } else {
                        File.WriteAllBytes(opts.Output, data);
                    }
                }

                return 0;
            }
            catch (Exception ex) when (ex is ArgumentException || ex is IOException) {
                Console.Error.WriteLine(ex.Message);
                return 1;
            }
        }

        private static int GetError(IEnumerable<Error> errors)
        {
            return 1;
        }
    }
}