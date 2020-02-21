// =====
//
// Copyright (c) 2013-2020 Timothy Baxendale
//
// =====
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Trithemius.Windows
{
    internal static class Dialog
    {
        public static string PromptInput(string message, string text)
        {
            return PromptInput(null, message, text, InputDialog.InputType.Any);
        }

        public static string PromptInput(string message, string text, InputDialog.InputType acceptedInput)
        {
            return PromptInput(null, message, text, acceptedInput);
        }

        public static string PromptInput(IWin32Window owner, string message, string text)
        {
            return PromptInput(owner, message, text, InputDialog.InputType.Any);
        }

        public static string PromptInput(IWin32Window owner, string message, string text, InputDialog.InputType acceptedInput)
        {
            using (InputDialog inputDialog = new InputDialog(owner, message, text, acceptedInput)) {
                if ((owner == null ? inputDialog.ShowDialog() : inputDialog.ShowDialog(owner)) == DialogResult.OK) {
                    return inputDialog.InputText;
                } else {
                    return null;
                }
            }
        }
    }
}
