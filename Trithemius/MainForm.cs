using System;
using System.Windows.Forms;
using System.Reflection;

namespace Trithemius
{
    public partial class MainForm : Form
    {

        #region Assembly Attribute Accessors

        public string AssemblyTitle
        {
            get {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0) {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "") {
                        return titleAttribute.Title;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public string AssemblyVersion
        {
            get {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public string AssemblyDescription
        {
            get {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (attributes.Length == 0) {
                    return "";
                }
                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        public string AssemblyProduct
        {
            get {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0) {
                    return "";
                }
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public string AssemblyCopyright
        {
            get {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0) {
                    return "";
                }
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        public string AssemblyCompany
        {
            get {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                if (attributes.Length == 0) {
                    return "";
                }
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }
        #endregion


        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            versionLabel.Text = string.Format("Trithemius Version [{0}]\n" +
                                              "{1}",
                                              ProductVersion,
                                              AssemblyCopyright);
        }
        
        private void encodeButton_Click(object sender, EventArgs e)
        {
            Hide();
            EncodeForm f = new EncodeForm();
            f.ShowDialog();
            Show();
        }

        private void decodeButton_Click(object sender, EventArgs e)
        {
            Hide();
            DecodeForm f = new DecodeForm();
            f.ShowDialog();
            Show();
        }
        
        private void encodeButton_MouseHover(object sender, EventArgs e)
        {
            toolTip1.ToolTipTitle = "Encoding";
            toolTip1.ToolTipIcon = ToolTipIcon.Info;
            toolTip1.Show("Encode hidden messages or files into images.", encodeButton);
        }

        private void decodeButton_MouseHover(object sender, EventArgs e)
        {
            toolTip1.ToolTipTitle = "Decoding";
            toolTip1.ToolTipIcon = ToolTipIcon.Info;
            toolTip1.Show("Decode hidden messages or files previously encoded in an image.", decodeButton);
        }
    }
}
