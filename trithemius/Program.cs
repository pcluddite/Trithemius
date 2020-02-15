// =====
//
// Copyright (c) 2013-2020 Timothy Baxendale
//
// =====
using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using Trithemius.Windows;

namespace Trithemius
{
    internal static class Program
    {
        [STAThread]
        public static void Main()
        {
            //Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new TrithemiusForm());
        }

        internal static Process TryStart(IWin32Window owner, ProcessStartInfo startInfo, bool displayErrors = true)
        {
            if (owner == null)
                throw new ArgumentNullException(nameof(owner));
            if (startInfo == null)
                throw new ArgumentNullException(nameof(startInfo));
            Contract.EndContractBlock();

            try {
                return Process.Start(startInfo);
            }
            catch (Exception ex) when (ex is InvalidOperationException || ex is IOException || ex is Win32Exception) {
                if (displayErrors)
                    MessageBox.Show(owner, ex.Message, "Trithemius", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        internal static void ShowError(Form form, string message)
        {
            MessageBox.Show(form, message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}