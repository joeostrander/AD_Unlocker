using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.DirectoryServices;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Account_Unlocker
{
    public partial class UserSearch : Form
    {

        public string selectedUserId = "";
        public string strServer = "";
        public string strLoginID = "";
        public string strLoginPassword = "";

        public UserSearch()
        {
            InitializeComponent();
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            StartSearch();
        }
        private void StartSearch() { 
            string strSearchString = textBoxUserString.Text;
            if (string.IsNullOrEmpty(strSearchString))
                return;

            listView1.Items.Clear();

            string strFilter = "(&(objectCategory=person)(objectClass=user)(|(sAMAccountName=*" + strSearchString + "*)(displayName=*"+strSearchString+"*)))";
            string[] arrProperties = { "samaccountName","displayname","givenname","sn" };
            SearchResultCollection results = AD.GetSearchResults(strServer, strFilter, arrProperties, strLoginID, strLoginPassword);

            if (results!=null)
            {
                if (results.Count==0)
                {
                    MessageBox.Show("Could not find \"" + strSearchString + "\"", "User not found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    foreach (SearchResult result in results)
                    {
                        string sam = result.Properties.Contains("samaccountname") ? (string)result.Properties["samaccountname"][0] : "";
                        string display = result.Properties.Contains("displayname") ? (string)result.Properties["displayname"][0] : "";
                        string first = result.Properties.Contains("givenname") ? (string)result.Properties["givenname"][0] : "";
                        string last = result.Properties.Contains("sn") ? (string)result.Properties["sn"][0] : "";

                        if (!string.IsNullOrEmpty(sam))
                        {
                            ListViewItem lvi = new ListViewItem(sam);
                            lvi.SubItems.Add(display);
                            lvi.SubItems.Add(first);
                            lvi.SubItems.Add(last);
                            listView1.Items.Add(lvi);
                        }

                    }
                }
                
            }
            else
            {
                //do nothing
            }


        }

        private void UserSearch_Load(object sender, EventArgs e)
        {
            
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            GetSelectedUserID();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void GetSelectedUserID()
        {
            if (listView1.SelectedIndices.Count > 0 )
            {
                int intSelectedIndex = listView1.SelectedIndices[0];
                selectedUserId = listView1.Items[intSelectedIndex].SubItems[0].Text;
            }
            else
            {
                selectedUserId = "";
            }
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            GetSelectedUserID();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }


        private void textBoxUserString_Enter(object sender, EventArgs e)
        {
            this.AcceptButton = null;
        }

        private void textBoxUserString_Leave(object sender, EventArgs e)
        {
            this.AcceptButton = buttonOk;
        }

        private void textBoxUserString_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                StartSearch();
            }
        }
    }
}
