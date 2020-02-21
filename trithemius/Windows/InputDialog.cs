// =====
//
// Copyright (c) 2013-2020 Timothy Baxendale
//
// =====
using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Trithemius.Windows
{
    public partial class InputDialog : Form
    {
        private const int MAX_LINE_SIZE = 234;

        public string InputText { get; private set; }
        public IWin32Window Owner { get; }

        public InputDialog(IWin32Window owner, string text, string title)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));
            if (title == null) throw new ArgumentNullException(nameof(title));
            InitializeComponent();
            Owner = owner;
            Text = title;
            SetText(text);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (Owner is Form ownerForm) {
                if (ownerForm.ShowIcon) {
                    Icon = ownerForm.Icon;
                    ShowIcon = true;
                }
            }
        }

        private void SetText(string text)
        {
            if (text.Length < MAX_LINE_SIZE) {
                lblPrompt.Text = text;
            } else {
                lblPrompt.Text = WordWrap(text);
            }
            this.ClientSize = new Size(lblPrompt.Width + lblPrompt.Location.X * 2, lblPrompt.Height + answerBox.Height + btnOK.Height + 16);
        }

        private static string WordWrap(string input)
        {
            StringBuilder output = new StringBuilder(input.Length * 2);
            StringBuilder word = new StringBuilder(input.Length);

            int lineCount = 0, wordCount = 0;

            for (int index = 0; index < input.Length + 1; ++index) {
                char c = index < input.Length ? input[index] : '\0';
                if (char.IsWhiteSpace(c) || c == '\0') {
                    if (lineCount + wordCount > MAX_LINE_SIZE) {
                        output.Append('\n');
                        lineCount = 0;
                    }
                    output.Append(word);
                    if (c != '\0') output.Append(c);
                    if (c == '\n') lineCount = 0;
                    lineCount += wordCount;
                    word.Clear();
                    wordCount = 0;
                } else {
                    word.Append(c);
                    ++wordCount;
                }
            }
            return output.ToString();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            InputText = answerBox.Text;
            base.OnFormClosing(e);
        }
    }
}
