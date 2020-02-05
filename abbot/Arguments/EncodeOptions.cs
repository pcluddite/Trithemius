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
