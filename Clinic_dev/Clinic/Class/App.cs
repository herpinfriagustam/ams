using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic
{
    class App
    {
        public static void ShowInfoMessage(string title, string message)
        {
            System.Windows.Forms.MessageBox.Show(title, message, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
        }

        public static void ShowInfoMessage(string message)
        {
            ShowInfoMessage("Information", message);
        }

        public static void ShowWarningMessage(string title, string message)
        {
            System.Windows.Forms.MessageBox.Show(title, message, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
        }

        public static void ShowWarningMessage(string message)
        {
            ShowWarningMessage("Warning", message);
        }

        public static void ShowErrorMessage(string title, string message)
        {
            System.Windows.Forms.MessageBox.Show(title, message, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
        }

        public static void ShowErrorMessage(string message)
        {
            ShowErrorMessage("Error", message);
        }

        public static System.Windows.Forms.DialogResult ShowConfirmMessage(string title, string message)
        {
            return System.Windows.Forms.MessageBox.Show(title, message, System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question);
        }

        public static System.Windows.Forms.DialogResult ShowConfirmMessage(string message)
        {
            return ShowConfirmMessage("Confirmation", message);
        }

        public static string GetString(object val, string defaultReturn = "")
        {
            if (val == null)
                return defaultReturn;

            return val.ToString();
        }
    }
}
