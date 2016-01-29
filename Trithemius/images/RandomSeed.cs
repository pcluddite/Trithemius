using System;
using System.Windows.Forms;

namespace Trithemius
{
    public partial class RandomSeed : Form
    {
        public int Result { get; set; }

        public RandomSeed()
        {
            InitializeComponent();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void generateButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Result = (int)seedNumericUpDown.Value;
            Close();
        }
    }
}
