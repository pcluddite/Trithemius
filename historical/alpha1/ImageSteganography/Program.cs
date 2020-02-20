using System;
using System.Linq;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace ImageSteganography
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Full());
        }


        static void main()
        {
            Console.WriteLine("IMGCODE [0.1 ALPHA]");
            Console.WriteLine("(c) Copyright 2012");
            Console.WriteLine();
            while (true)
            {
                Console.Write("Encode? Decode? Quit? [E/D/Q] ");
                switch (Console.ReadKey().KeyChar)
                {
                    case 'e':
                        EncodeMode();
                        break;
                    case 'd':
                        DecodeMode();
                        break;
                    case 'q':
                        return;
                    default:
                        Console.WriteLine(" - INVALID");
                        continue;
                }
            }
        }

        public static Bitmap Encode(Bitmap img, byte[] msg, int ARGB, ref int prog)
        {
            int x = 0, y = 0;
            string binary = "";
            foreach (byte bt in BitConverter.GetBytes(msg.GetUpperBound(0)))
            {
                binary += Convert.ToString(bt, 2).PadLeft(8, '0');
            }
            foreach (byte bt in msg)
            {
                binary += Convert.ToString(bt, 2).PadLeft(8, '0');
            }
            int b = 0;
            prog = 0;
            foreach (char c in binary)
            {
                if (x >= img.Width) { x = 0; y++; }
                Color col = img.GetPixel(x, y);
                int argb;
                switch (ARGB)
                {
                    case 0: argb = col.A; break;
                    case 1: argb = col.R; break;
                    case 2: argb = col.G; break;
                    case 3: argb = col.B; break;
                    default: argb = col.R; break;
                }
                if (c == '1')
                {
                    if ((argb& 1) != 0)
                    {
                        if (argb == 255) { b = 254; }
                        else { b = argb + 1; }
                    }
                    else { b = argb; }
                }
                if (c == '0')
                {
                    if ((argb & 1) == 0)
                    {
                        b = argb + 1;
                    }
                    else { b = argb; }
                }
                switch (ARGB)
                {
                    case 0:
                        img.SetPixel(x, y, Color.FromArgb(b, col.R, col.G, col.B));
                        break;
                    case 1:
                        img.SetPixel(x, y, Color.FromArgb(col.A, b, col.G, col.B));
                        break;
                    case 2:
                        img.SetPixel(x, y, Color.FromArgb(col.A, col.R, b, col.B));
                        break;
                    case 3:
                        img.SetPixel(x, y, Color.FromArgb(col.A, col.R, col.G, b));
                        break;
                }
                x++;
                prog++;
            }

            return img;
        }

        static void EncodeMode()
        {
            Encode:
            Console.Write("\r\nIMAGE FILE [] ");
            string file;
            if (!File.Exists(file = Console.ReadLine()))
            {
                Console.Write(" - INVALID PATH"); goto Encode;
            }
            Console.WriteLine("Reading File... (Let's hope you didn't pick a big one)");
            Console.WriteLine();
            Bitmap img = (Bitmap)Image.FromStream(new MemoryStream(File.ReadAllBytes(file)));
            Console.WriteLine("IMAGE RESOLUTION: {0}x{1}", img.Width, img.Height);
            int size;
            Console.WriteLine("MAXIMUM TEXT SIZE: {0} characters", size = ((img.Width * img.Height) / 8) - 32);
            byte[] data;
            Console.WriteLine();
            Console.Write("File or text? [F/T] ");
            while (true)
            {
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.T:
                        string msg;
                        while (true)
                        {
                            Console.Write("\r\nMESSAGE [] ");
                            if ((msg = Console.ReadLine()).Length > size) { Console.WriteLine("MESSAGE SIZE OVERLOAD"); continue; }
                            break;
                        }
                        data = Encoding.ASCII.GetBytes(msg);
                        break;
                    case ConsoleKey.F:
                        while (true)
                        {
                            Console.Write("\r\nFILE TO ENCODE [] ");
                            if (!File.Exists(file = Console.ReadLine()))
                            {
                                Console.Write(" - INVALID PATH"); continue;
                            }
                            break;
                        }
                        data = File.ReadAllBytes(file);
                        break;
                    default:
                        Console.WriteLine(" - INVALID");
                        continue;
                }
                break;
            }

            while (true)
            {
                Console.Write("Encode in [A/R/G/B] ");
                int prog = 0;
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.A:
                        Console.WriteLine("\r\nEncoding...");
                        Encode(img, data, 0, ref prog);
                        break;
                    case ConsoleKey.R:
                        Console.WriteLine("\r\nEncoding...");
                        Encode(img, data, 1, ref prog);
                        break;
                    case ConsoleKey.G:
                        Console.WriteLine("\r\nEncoding...");
                        Encode(img, data, 2, ref prog);
                        break;
                    case ConsoleKey.B:
                        Console.WriteLine("\r\nEncoding...");
                        Encode(img, data, 3, ref prog);
                        break;
                    default:
                        Console.WriteLine(" - INVALID");
                        continue;
                }
                break;
            }
            Console.WriteLine("Done!");

            Console.Write("OUTPUT [] ");
            img.Save(Console.ReadLine(), System.Drawing.Imaging.ImageFormat.Png);
            return;
        }

        static void DecodeMode()
        {
        Decode:
            Console.Write("\r\nIMAGE FILE [] ");
            string file;
            if (!File.Exists(file = Console.ReadLine()))
            {
                Console.Write(" - INVALID PATH"); goto Decode;
            }

            Console.WriteLine("Reading File... (Let's hope you didn't pick a big one)");
            Bitmap img = (Bitmap)Image.FromStream(new MemoryStream(File.ReadAllBytes(file)));
            byte[] data;
            int prog = 0;
            while (true)
            {
                Console.Write("Decode from [A/R/G/B] ");
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.A:
                        Console.WriteLine("\r\nDecoding...");
                        data = Decode(img, 0, ref prog);
                        break;
                    case ConsoleKey.R:
                        Console.WriteLine("\r\nDecoding...");
                        data = Decode(img, 1, ref prog);
                        break;
                    case ConsoleKey.G:
                        Console.WriteLine("\r\nDecoding...");
                        data = Decode(img, 2, ref prog);
                        break;
                    case ConsoleKey.B:
                        Console.WriteLine("\r\nDecoding...");
                        data = Decode(img, 3, ref prog);
                        break;
                    default:
                        Console.WriteLine(" - INVALID");
                        continue;
                }
                break;
            }
            Console.WriteLine("Done!");
            Console.WriteLine();
            while (true)
            {
                Console.Write("Decode as file or text? [F/T] ");
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.T:
                        Console.WriteLine("\r\nDecoded message:\r\n{0}", Encoding.ASCII.GetString(data));
                        break;
                    case ConsoleKey.F:
                        Console.Write("\r\nOUTPUT [] ");
                        file = Console.ReadLine();
                        Console.WriteLine("\r\nSaving...");
                        File.WriteAllBytes(file, data);
                        Console.WriteLine("Done!");
                        break;
                    default:
                        Console.WriteLine(" - INVALID"); continue;
                }
                break;
            }
            return;
        }

        public static byte[] Decode(Bitmap img, int ARGB, ref int prog)
        {
            string binary = "";
            int x = 0, y = 0;
            while ((prog = binary.Length) <= 32)
            {
                if (x >= img.Width) { x = 0; y++; }
                Color c = img.GetPixel(x, y);
                switch (ARGB)
                {
                    case 0:
                        if ((c.A & 1) == 0) { binary += "1"; } else { binary += "0"; } break;
                    case 1:
                        if ((c.R & 1) == 0) { binary += "1"; } else { binary += "0"; } break;
                    case 2:
                        if ((c.G & 1) == 0) { binary += "1"; } else { binary += "0"; } break;
                    case 3:
                        if ((c.B & 1) == 0) { binary += "1"; } else { binary += "0"; } break;
                }
                x++;
            }

            int size = BitConverter.ToInt32(getByte(binary.Remove(32)), 0) + 1;

            while ((prog = binary.Length) - 32 <= size * 8)
            {
                if (x >= img.Width) { x = 0; y++; }
                Color c = img.GetPixel(x, y);
                switch (ARGB)
                {
                    case 0:
                        if ((c.A & 1) == 0) { binary += "1"; } else { binary += "0"; } break;
                    case 1:
                        if ((c.R & 1) == 0) { binary += "1"; } else { binary += "0"; } break;
                    case 2:
                        if ((c.G & 1) == 0) { binary += "1"; } else { binary += "0"; } break;
                    case 3:
                        if ((c.B & 1) == 0) { binary += "1"; } else { binary += "0"; } break;
                }
                x++;
            }
            return getByte(binary.Remove(0, 32).Remove(size * 8));
        }

        public static byte[] getByte(string input)
        {
            int numOfBytes = input.Length / 8;
            byte[] bytes = new byte[numOfBytes];
            for (int i = 0; i < numOfBytes; ++i)
            {
                bytes[i] = Convert.ToByte(input.Substring(8 * i, 8), 2);
            }
            return bytes;
        }
    }
}
