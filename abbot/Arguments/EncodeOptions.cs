// =====
//
// Copyright (c) 2013-2020 Timothy Baxendale
//
// =====
using CommandLine;

namespace Abbot.Arguments
{
    [Verb("encode", HelpText = "encode a message in an image")]
    internal class EncodeOptions : Options
    {
        [Option('m', "message", Required = false, HelpText = "the message to encode")]
        public string Message { get; set; }

        [Option('f', "file", Required = false, HelpText = "the path of the file to encode")]
        public string File { get; set; }

        [Option('o', "out", Required = true, HelpText = "the path of the file to write the encoded image")]
        public string Output { get; set; }
    }
}
