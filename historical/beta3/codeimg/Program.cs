using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;

namespace codeimg
{
    class Program
    {
        static void Main(string[] args)
        {
            /* Parameters
             * codeimg /[encode | decode]:imagepath data [/bits:n] [/color:c] [/seed:s] [/key:] [/output:[filepath] ]
             */
            if (args[0].ToLower().StartsWith("/encode")) { Encode(parse(args)); }
            else if (args[0].ToLower().StartsWith("/decode")) { Decode(parse(args)); }
            else { error("syntax"); }
        }

        private static Dictionary<string, string> parse(string[] args)
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            for(int i = 0; i < args.Length; i++)
            {
                if (args[i].ToLower().StartsWith("/encode:")) { param.Add("encode", args[i].Remove(0, 8)); }
                if (args[i].ToLower().StartsWith("/decode:")) { param.Add("decode", args[i].Remove(0, 8)); }
                if (args[i].ToLower().StartsWith("/data:")) { param.Add("data", args[i].Remove(0, 8)); }
                if (args[i].ToLower().StartsWith("/color:")) { param.Add("color", args[i].Remove(0, 7)); }
                if (args[i].ToLower().StartsWith("/bits:")) { param.Add("bits", args[i].Remove(0, 6)); }
                if (args[i].ToLower().StartsWith("/seed:")) { param.Add("seed", args[i].Remove(0, 6)); }
                if (args[i].ToLower().StartsWith("/key:")) { param.Add("key", args[i].Remove(0, 5)); }
                if (args[i].ToLower().StartsWith("/output:")) { param.Add("output", args[i].Remove(0, 5)); }
            }
            if (!param.Keys.Contains("/encode:") &&
                !param.Keys.Contains("/decode:")) { error("syntax"); }
            if (!param.Keys.Contains("/data:")) { error("notfound"); }
            if (!param.Keys.Contains("/color:")) { param.Add("color", "red"); }
            if (!param.Keys.Contains("/bits:")) { param.Add("bits", "1"); }
            if (!param.Keys.Contains("/seed:")) { param.Add("seed", "1"); }
            if (!param.Keys.Contains("/key:")) { param.Add("key", ""); }
            if (!param.Keys.Contains("/output:")) { param.Add("key", ""); }
            return param;
        }

        private static void error(string type)
        {
            switch (type)
            {
                case "syntax": 
                    Console.WriteLine("The syntax of the command is incorrect.");
                    break;
                case "notfound":
                    Console.WriteLine("No data was specified for a given parameter.");
                    break;
                case "invalid":
                    Console.WriteLine("One of the values entered was invalid.");
                    break;
            }
            Environment.Exit(0);
        }

        private static int getColor(string s)
        {
            switch (s)
            {
                case "alpha":
                    return 0;
                case "red":
                    return 1;
                case "blue":
                    return 2;
                case "green":
                    return 3;
                default:
                    error("invalid");
                    return 0;
            }
        }

        private static void Encode(Dictionary<string,string> args)
        {
            if (args["encode"].Trim().CompareTo("") == 0) { error("notfound"); }

            try
            {
                Bitmap img = (Bitmap)Bitmap.FromFile(args["encode"]);
                int ARGB = getColor(args["color"]);
                byte[] msg = radioButton1.Checked ?
                    Encoding.ASCII.GetBytes(message) :
                    System.IO.File.ReadAllBytes(openFileDialog2.FileName);

                if (textBox1.Text.CompareTo("") != 0)
                {
                    msg = Crypto.EncryptStringAES(msg, textBox1.Text);
                }

                int[] seed = new int[] { 1 };

                if (textBox2.Text.CompareTo("") != 0)
                {
                    Int64 s;
                    if (!Int64.TryParse(textBox2.Text, out s))
                    {
                        error = true;
                        MessageBox.Show("The seed entered is not valid.", Text,
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    seed = Array.ConvertAll<char, int>(
                        textBox2.Text.ToCharArray(), new Converter<char, int>(CharToInt));
                }

                int lsb = (int)numericUpDown2.Value;

                byte[] data = new byte[5 + msg.GetUpperBound(0)];
                BitConverter.GetBytes(msg.GetUpperBound(0)).CopyTo(data, 0);
                msg.CopyTo(data, 4);
                string[] binary = Split(string.Join("",
                    data.Select(l => Convert.ToString(l, 2).PadLeft(8, '0')).ToArray<string>()),
                    lsb).ToArray();

                int b = 0;
                int i = seed[0] - 1;

                List<byte> nData = new List<byte>();
                Color[] allPixels = getPixels(img, ARGB, inflateSize(msgSize + 1) * 8);
                Dictionary<int, byte> changes = new Dictionary<int, byte>();
                int ln = 0;
                foreach (string c in binary)
                {
                    int[] color = new int[] { allPixels[i].A, 
                        allPixels[i].R, allPixels[i].G, allPixels[i].B };
                    int argb = color[ARGB];
                    string pixelBinary = Convert.ToString(argb, 2).PadLeft(8, '0');

                    b = Convert.ToInt32(pixelBinary.Remove(8 - lsb, lsb) + c, 2);

                    if (b != argb) { changes.Add(i - 1, (byte)argb); }
                    color[ARGB] = b;
                    allPixels[i] = Color.FromArgb(color[0], color[1], color[2], color[3]);
                    i += seed[ln % seed.Length];
                    ln++;
                }
                img = setPixels(img, allPixels, seed);
                int width = img.Width, height = img.Height;
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                img.Dispose();
                Bitmap.FromStream(ms).Save(saveFileDialog2.FileName, System.Drawing.Imaging.ImageFormat.Png);
                if (checkBox1.Checked)
                {
                    Invoke(new MethodInvoker(delegate { savelog(ARGB, changes, PixelNumber(width, height, width)); }));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message +
                    ex.TargetSite
                    , Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                error = true;
            }
        }

        private static void Decode(Dictionary<string, string> args)
        {
        }
    }
}
