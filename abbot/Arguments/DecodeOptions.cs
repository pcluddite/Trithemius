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

using Monk.Imaging;

namespace Abbot.Arguments
{
    [Verb("decode", HelpText = "decode a message from an image")]
    internal class DecodeOptions : Options
    {
        [Option('o', "out", Required = false, HelpText = "Sets a path to write the decoded message")]
        public string Output { get; set; }

        [Option("legacy", Required = false, Hidden = true, HelpText = "Enables options for legacy decoding. This overrides any other settings.")]
        public bool Legacy { get; set; }

        public override Steganographer BuildTrithemius()
        {
            Steganographer trithemius = base.BuildTrithemius();
            if (Legacy) {
                trithemius.SetLegacyOptions();
            }
            return trithemius;
        }
    }
}
