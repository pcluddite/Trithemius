// =====
//
// Copyright (c) 2013-2020 Timothy Baxendale
//
// =====
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

        public override SteganographyInfo BuildTrithemius()
        {
            SteganographyInfo trithemius;

            if (Legacy) {
                trithemius = SteganographyInfo.PresetsB0003;

                if (Alpha) {
                    trithemius.Colors.Add(PixelColor.Alpha);
                } else if (Red) {
                    trithemius.Colors.Add(PixelColor.Red);
                } else if (Green) {
                    trithemius.Colors.Add(PixelColor.Green);
                } else if (Blue) {
                    trithemius.Colors.Add(PixelColor.Blue);
                }
            } else {
                trithemius = base.BuildTrithemius();
            }

            return trithemius;
        }
    }
}
