using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Trithemius
{
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
        }

        char[] alphabet = new char[] { 'A', 'B', 'C', 'D', 'E',
                'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P',
            'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

        private void button1_Click(object sender, EventArgs e)
        {
            if (!Regex.IsMatch(textBox1.Text, @"^[a-zA-Z ]+$") ||
            !Regex.IsMatch(textBox2.Text, @"^[a-zA-Z]+$")) {
                MessageBox.Show(this, "No special characters are allowed in the message or key box.",
                    Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; }
            char[] key = textBox2.Text.ToUpper().ToCharArray();
            char[] txt = textBox1.Text.ToUpper().Replace(" ", "").ToCharArray();
            StringBuilder sb = new StringBuilder();
            int i = 0;
            foreach (char c in txt)
            {
                sb.Append(alphabet[
                    (indexOf(c) + indexOf(key[i % key.Length])) 
                    % 26]);
                i++;
            }
            textBox3.Text = sb.ToString();
        }

        int indexOf(char c)
        {
            return alphabet.ToList().IndexOf(c);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!Regex.IsMatch(textBox1.Text, @"^[a-zA-Z]+$") ||
            !Regex.IsMatch(textBox2.Text, @"^[a-zA-Z]+$"))
            {
                MessageBox.Show(this, "No special characters are allowed in the message or key box.",
                    Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            char[] key = textBox2.Text.ToUpper().ToCharArray();
            char[] txt = textBox1.Text.ToUpper().ToCharArray();
            StringBuilder sb = new StringBuilder();
            int i = 0;
            foreach (char c in txt)
            {
                int index = indexOf(c) - indexOf(key[i % key.Length]);
                if (index < 0) { index = 26 + index; }
                sb.Append(alphabet[
                    (index)
                    % 26]);
                i++;
            }
            textBox3.Text = sb.ToString();
        }
    }
}
