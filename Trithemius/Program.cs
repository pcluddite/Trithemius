using System;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Trithemius
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new TrithemiusForm());
        }

        public static string SizeToString(double size)
        {
            int unit = 0;
            if (size >= 1024) { size /= 1024; unit = 1; }
            if (size >= 1024) { size /= 1024; unit = 2; }
            if (size >= 1024) { size /= 1024; unit = 3; }
            string n = size.ToString("0.##");
            switch (unit)
            {
                case 0: n += " B"; break;
                case 1: n += " KB"; break;
                case 2: n += " MB"; break;
                case 3: n += " GB"; break;
            }
            return n;
        }
    }
}
