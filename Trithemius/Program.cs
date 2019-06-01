/**
 *  Trithemius
 *  Copyright (C) Timothy Baxendale
 *
 *  This program is free software; you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation; either version 2 of the License, or
 *  (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License along
 *  with this program; if not, write to the Free Software Foundation, Inc.,
 *  51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA.
**/
using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.ComponentModel;
using System.Diagnostics.Contracts;

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
            catch(Exception ex) when (ex is InvalidOperationException || ex is IOException || ex is Win32Exception) {
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
