using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Trithemius
{
    public partial class Form8 : Form
    {
        public Form8()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                byte[] data = new byte[255];
                switch (comboBox1.SelectedIndex)
                {
                    case 0: data = Encoding.ASCII.GetBytes(textBox1.Text); break;
                    case 1: data = Encoding.Unicode.GetBytes(textBox1.Text); break;
                    case 2: data = Encoding.UTF7.GetBytes(textBox1.Text); break;
                    case 3: data = Encoding.UTF8.GetBytes(textBox1.Text); break;
                    case 4: data = Encoding.UTF32.GetBytes(textBox1.Text); break;
                    case 5: data = toAlphabet(textBox1.Text); break;
                }
                switch (comboBox2.SelectedIndex)
                {
                    case 0: textBox1.Text =
                        string.Join(" ", Array.ConvertAll<byte, string>(data,
                        new Converter<byte, string>(byteToBin))); break;
                    case 1: textBox1.Text =
                        string.Join(" ", Array.ConvertAll<byte, string>(data,
                        new Converter<byte, string>(byteToHex))); break;
                    case 2: textBox1.Text =
                        string.Join(" ", Array.ConvertAll<byte, string>(data,
                        new Converter<byte, string>(byteToString))); break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string byteToHex(byte b)
        {
            return Convert.ToString(b, 16).PadLeft(2, '0').ToUpper();
        }

        private string byteToBin(byte b)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 5:
                    return Convert.ToString(b, 2).PadLeft(5, '0');
                case 0:
                    return Convert.ToString(b, 2).PadLeft(7, '0');
                default:
                    return Convert.ToString(b, 2).PadLeft(8, '0');

            }
        }

        private string byteToString(byte b)
        {
            return b.ToString();
        }

        private byte[] toAlphabet(string s)
        {
            List<char> key = new List<char>();
            key.AddRange(new char[] { ' ', 'A', 'B', 'C', 'D', 'E', 'F', 'G',
                'H', 'I', 'J', 'K','L','M','N','O','P','Q','R','S','T',
                'U','V','W','X','Y','Z','.', '!', '?' });
            List<byte> data = new List<byte>();
            foreach (char c in s.ToUpper())
            {
                data.Add((byte)key.IndexOf(c));
            }
            return data.ToArray();
        }

        private string fromAlphabet(byte[] s)
        {
            List<string> key = new List<string>();
            key.AddRange(new string[] { " ", "A", "B", "C", "D", "E", "F", "G",
                "H", "I", "J", "K","L","M","N","O","P","Q","R","S","T",
                "U","V","W","X","Y","Z",".", "!", "?" });
            List<string> data = new List<string>();
            foreach (byte c in s)
            {
                data.Add(key[(int)c]);
            }
            return string.Join("", data.ToArray());
        }

        byte hexToByte(string s)
        {
            return Convert.ToByte(s, 16);
        }

        byte binToByte(string s)
        {
            return Convert.ToByte(s, 2);
        }

        byte decToByte(string s)
        {
            return byte.Parse(s);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                byte[] data = new byte[255];
                switch (comboBox2.SelectedIndex)
                {
                    case 0: data =
                        Array.ConvertAll<string, byte>(
                        textBox1.Text.Split(' '), new Converter<string, byte>(binToByte)); break;
                    case 1: data =
                        Array.ConvertAll<string, byte>(
                        textBox1.Text.Split(' '), new Converter<string, byte>(hexToByte)); break;
                    case 2: data =
                        Array.ConvertAll<string, byte>(textBox1.Text.Split(' '),
                        new Converter<string, byte>(decToByte)); break;
                }

                switch (comboBox1.SelectedIndex)
                {
                    case 0: textBox1.Text = Encoding.ASCII.GetString(data); break;
                    case 1: textBox1.Text = Encoding.Unicode.GetString(data); break;
                    case 2: textBox1.Text = Encoding.UTF7.GetString(data); break;
                    case 3: textBox1.Text = Encoding.UTF8.GetString(data); break;
                    case 4: textBox1.Text = Encoding.UTF32.GetString(data); break;
                    case 5: textBox1.Text = fromAlphabet(data); break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
