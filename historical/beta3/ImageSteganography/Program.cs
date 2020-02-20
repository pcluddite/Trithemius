using System;
using System.Text;
using System.Windows.Forms;

namespace Trithemius
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form3());
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

        public static string simplifySize(decimal size)
        {
            int unit = 0;
            if (size >= 1000) { size /= (decimal)1024; unit = 1; }
            if (size >= 1000) { size /= (decimal)1024; unit = 2; }
            if (size >= 1000) { size /= (decimal)1024; unit = 3; }
            StringBuilder sb = new StringBuilder(Math.Round(size, 1).ToString());
            switch (unit)
            {
                case 0: sb.Append(" Bytes"); break;
                case 1: sb.Append(" Kilobytes"); break;
                case 2: sb.Append(" Megabytes"); break;
                case 3: sb.Append(" Gigabytes"); break;
            }
            return sb.ToString();
        }
    }
}
