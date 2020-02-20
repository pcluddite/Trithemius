using System.Windows.Forms;

namespace Trithemius
{
    public partial class Finished : Form
    {
        public Finished(string text)
        {
            InitializeComponent();
            textBox2.Text = text;
        }
    }
}
