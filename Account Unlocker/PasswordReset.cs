using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Account_Unlocker
{
    public partial class PasswordReset : Form
    {
        public string NewPassword;
        public string UserID;
        public bool MustChange = false;

        public PasswordReset()
        {
            InitializeComponent();
        }

        private void PasswordReset_Load(object sender, EventArgs e)
        {
            this.Text += " - " + UserID;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            string newPass = textBoxNewPassword.Text;
            string confirmPass = textBoxConfirmPassword.Text;

            if (string.IsNullOrEmpty(newPass))
            {
                MessageBox.Show("Enter a value for new password.", "New Password", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                textBoxNewPassword.Focus();
                return;
            }

            if (string.IsNullOrEmpty(confirmPass))
            {
                MessageBox.Show("Enter a value for confirm password.", "Confirm Password", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                textBoxNewPassword.Focus();
                return;
            }

            if (newPass != confirmPass)
            {
                MessageBox.Show("Passwords do not match!", "Password Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                textBoxConfirmPassword.Focus();
                textBoxConfirmPassword.SelectAll();
                return;
            }

            this.NewPassword = newPass;
            this.MustChange = checkBoxMustChange.Checked;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxHidePasswords.Checked)
            {
                textBoxNewPassword.PasswordChar = '*';
                textBoxConfirmPassword.PasswordChar = '*';
            }
            else
            {
                textBoxNewPassword.PasswordChar = '\0';
                textBoxConfirmPassword.PasswordChar = '\0';
            }
        }
    }
}
