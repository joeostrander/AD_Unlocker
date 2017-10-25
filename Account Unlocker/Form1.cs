using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.DirectoryServices.AccountManagement;
using System.Xml;
using System.Text.RegularExpressions;

namespace Account_Unlocker
{
    public partial class Form1 : Form
    {

        /*
         TO DO:  
         add password expires column
         add option to search by name
               
             */


        public struct MyServer
        {
            public string Name;
            public string SiteName;
        }

        public struct LockoutData
        {
            public string user_id;
            public string user_state;
            public int bad_pwd_count;
            public string last_bad_pwd;
            public string pwd_last_set;
            public string lockout_time;
            public string orig_lock;
            public string password_age;
        }




        public Form1()
        {
            InitializeComponent();
        }

        private void comboBoxGetSites_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox myBox = (ComboBox)sender;
            string boxText = myBox.Text;
            if (boxText.ToLower().Contains("get sites"))
            {
                FillSites();
            }
            
        }

        private void FillSites() { 
           

            comboBoxSite.Items.Clear();
            //myBox.Items.AddRange(listSites.ToArray());

            //List<string> listSites = new List<string>();
            List<ActiveDirectorySite> listSites = GetSites();

            foreach (ActiveDirectorySite site in listSites)
            {
                comboBoxSite.Items.Add(site.Name);
            }
            
            comboBoxSite.Items.Add("Get sites...");
            comboBoxSite.Items.Add("");





        }
    




        private List<MyServer> GetDomainControllers()
        {
            //List<DirectoryServer> DC = new List<DirectoryServer>();
            List<MyServer> DC = new List<MyServer> ();

            //string strDomain = comboBoxDomain.Text;
            string strSite = comboBoxSite.Text;
            //string strConfigurationNamingContext;

            List<ActiveDirectorySite> listSites = GetSites();
            foreach (ActiveDirectorySite site in listSites)
            {
                bool boolAdd = true;
                if (!string.IsNullOrEmpty(strSite))
                {
                    if (site.Name != strSite)
                    {
                        boolAdd = false;
                    }
                }
                if (boolAdd)
                {
                    foreach (DirectoryServer srv in site.Servers)
                    {
                        MyServer mySrv = new MyServer();
                        mySrv.Name = srv.Name;
                        mySrv.SiteName = site.Name;
                        DC.Add(mySrv);
                    }
                }
            }
            
            return DC;

        }


        private List<ActiveDirectorySite> GetSites()
        {
            List<ActiveDirectorySite> sites = new List<ActiveDirectorySite>();
            string strDomain = comboBoxDomain.Text;
            string strLoginID = textBoxUsername.Text;
            string strLoginPassword = textBoxPassword.Text;
            string strConfigurationNamingContext;


            string strSite = comboBoxSite.Text;


            if (!string.IsNullOrEmpty(strDomain))
            {
                DirectoryEntry ent = AD.GetDirectoryEntry("LDAP://" + strDomain + "/RootDSE",strLoginID,strLoginPassword);
                if (ent == null)
                    return sites;

                try
                {
                    if (ent.Properties.Contains("configurationNamingContext"))
                    {
                        strConfigurationNamingContext = ent.Properties["configurationNamingContext"][0].ToString();
                        string strSitesContainer = "LDAP://" + strDomain + "/cn=Sites," + strConfigurationNamingContext;
                        Forest x = AD.GetForest(strDomain, strLoginID, strLoginPassword);
                        foreach (ActiveDirectorySite site in x.Sites)
                        {
                            sites.Add(site);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Get sites - Exception", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return sites;
                }

            }

            return sites;
        }
        

        private void buttonGo_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxUserIDSearch.Text))
                return;

            if (string.IsNullOrEmpty(comboBoxSite.Text))
            {
                DialogResult ret = MessageBox.Show("Search ALL domain controlers?", "Search All DCs?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (ret!= DialogResult.Yes)
                {
                    comboBoxSite.Focus();
                    return;
                }
            }

            EnableControls(false);

            //Clear list
            listView1.Items.Clear();
            
            //Get DC list
            List<MyServer> listServers = GetDomainControllers();

            foreach(MyServer srv in listServers)
            {
                Console.WriteLine(srv.Name);
            }


            //loop through DCs in list, querying each server for user info
            foreach (MyServer DC in listServers)
            {
                string dcShortName;
                if (DC.Name.Contains("."))
                {
                    dcShortName = DC.Name.Substring(0, DC.Name.IndexOf("."));
                }
                else
                {
                    dcShortName = DC.Name;
                }

                //Server, Site, User State, Bad Pwd Count, Last Bad Pwd, Pwd Last, Lockout Time, Orig Lock
                LockoutData data = GetLockoutData(DC.Name);
                if (string.IsNullOrEmpty(data.user_id))
                {
                    MessageBox.Show("Could not find data for:  " + textBoxUserIDSearch.Text,"Not found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    EnableControls(true);
                    return;
                }

                ListViewItem lvi = new ListViewItem(DC.Name);
                lvi.SubItems.Add(dcShortName);
                lvi.SubItems.Add(data.user_id);
                lvi.SubItems.Add(DC.SiteName);
                lvi.SubItems.Add(data.user_state);
                lvi.SubItems.Add(data.bad_pwd_count.ToString());
                lvi.SubItems.Add(data.last_bad_pwd);
                lvi.SubItems.Add(data.pwd_last_set);
                lvi.SubItems.Add(data.lockout_time);
                lvi.SubItems.Add(data.orig_lock);
                lvi.SubItems.Add(data.password_age);


                listView1.Items.Add(lvi);
            }

            EnableControls(true);
        }

        private void EnableControls(bool v)
        {
            buttonGo.Enabled = v;
            comboBoxDomain.Enabled = v;
            textBoxUsername.Enabled = v;
            textBoxPassword.Enabled = v;
            comboBoxSite.Enabled = v;
            textBoxUserIDSearch.Enabled = v;
            listView1.Enabled = v;
        }

        private LockoutData GetLockoutData(string server)
        {
            //string strDomain = comboBoxDomain.Text;
            string strLoginID = textBoxUsername.Text;
            string strLoginPassword = textBoxPassword.Text;

            string user_id = textBoxUserIDSearch.Text;

            LockoutData ld = new LockoutData();

            if (string.IsNullOrEmpty(user_id))
                return ld;

            //DirectoryEntry root = AD.GetDirectoryEntry("LDAP://" + server,strLoginID,strLoginPassword);
            //DirectorySearcher searcher = new DirectorySearcher(root);



            //string strFilter = "(&(objectCategory=person)(objectClass=user)(sAMAccountName=" + user_id + "))";

            //searcher.PageSize = 1000;
            //searcher.Filter = strFilter;
            //searcher.PropertiesToLoad.Add("userAccountControl");
            //searcher.PropertiesToLoad.Add("pwdLastSet");
            //searcher.PropertiesToLoad.Add("msDS-ReplAttributeMetaData");

            //SearchResult result;
            //try
            //{
            //    result = searcher.FindOne();
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //    return ld;
            //}

            string strFilter = "(&(objectCategory=person)(objectClass=user)(sAMAccountName=" + user_id + "))";
            string[] props = { "userAccountControl", "pwdLastSet", "msDS-ReplAttributeMetaData" };
            SearchResultCollection results = AD.GetSearchResults(server, strFilter, props, strLoginID, strLoginPassword);
            
            if (results != null)
            {
                if (results.Count == 0)
                {
                    //MessageBox.Show("Not found:  " + user_id, "Not found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    SearchResult result = results[0];

                    ld.user_id = user_id;

                    if (result.Properties.Contains("userAccountControl"))
                    {
                        int userAccountControlValue = (int)result.Properties["userAccountControl"][0];
                        AD.ADS_USER_FLAG_ENUM userAccountControl = (AD.ADS_USER_FLAG_ENUM)userAccountControlValue;

                        ld.user_state = (userAccountControlValue & (int)AD.ADS_USER_FLAG_ENUM.ADS_UF_LOCKOUT) == (int)AD.ADS_USER_FLAG_ENUM.ADS_UF_LOCKOUT ? "Locked" : "Not Locked";
                    }

                    if (result.Properties.Contains("pwdLastSet"))
                    {
                        long lngPasswordChanged = (long)result.Properties["pwdLastSet"][0];
                        DateTime dtmLastSet = DateTime.FromFileTime(lngPasswordChanged);
                        ld.pwd_last_set = dtmLastSet.ToString();
                        int intDaysOld = (int)(DateTime.Now - dtmLastSet).TotalDays;
                        string suffix = intDaysOld > 1 ? " days" : " day";
                        ld.password_age = intDaysOld.ToString() + suffix;
                    }


                    //Console.WriteLine(xml);

                    PrincipalContext principalContext = AD.GetPrincipalContext(server, strLoginID, strLoginPassword);
                    UserPrincipal userPrincipal = UserPrincipal.FindByIdentity(principalContext, textBoxUserIDSearch.Text);

                    if (userPrincipal.IsAccountLockedOut())
                    {
                        ld.user_state = "Locked";

                        ld.bad_pwd_count = userPrincipal.BadLogonCount;
                        //userPrincipal.UnlockAccount()
                        //userPrincipal.SetPassword()
                        //userPrincipal.Save()
                        //userPrincipal.RefreshExpiredPassword()
                        //userPrincipal.LastBadPasswordAttempt
                        foreach (string property in result.Properties["msDS-ReplAttributeMetaData"])
                        {
                            if (property.ToLower().Contains("lockouttime") && property.ToLower().Contains("pszlastoriginatingdsadn"))
                            {
                                XmlDocument xmlDoc = new XmlDocument();
                                xmlDoc.LoadXml(property);

                                Console.WriteLine("*****************");
                                Console.WriteLine(xmlDoc.SelectSingleNode("DS_REPL_ATTR_META_DATA/pszLastOriginatingDsaDN").InnerText);
                                Console.WriteLine(xmlDoc.SelectSingleNode("DS_REPL_ATTR_META_DATA/pszLastOriginatingDsaDN").Value);
                                Console.WriteLine("*****************");

                                string strPattern = "CN=NTDS Settings,CN=(?<servername>[^,]+),";
                                Regex objRegEx = new Regex(strPattern, RegexOptions.IgnoreCase);
                                MatchCollection colMatches = objRegEx.Matches(property);
                                if (colMatches.Count > 0)
                                {
                                    ld.orig_lock = colMatches[0].Groups["servername"].Value;
                                }

                            }

                        }

                    }
                    else
                    {
                        ld.user_state = "Not Locked";
                        ld.orig_lock = "N/A";
                        //ld.lockout_time = "N/A";
                        //long lngLockoutTime = (long)result.Properties["lockouttime"][0];
                        //ld.lockout_time = DateTime.FromFileTime(lngLockoutTime).ToLocalTime().ToString();


                    }
                    ld.lockout_time = userPrincipal.AccountLockoutTime == null ? "N/A" : ((DateTime)userPrincipal.AccountLockoutTime).ToLocalTime().ToString();
                    ld.last_bad_pwd = userPrincipal.LastBadPasswordAttempt == null ? "None" : ((DateTime)userPrincipal.LastBadPasswordAttempt).ToLocalTime().ToString();


                    DirectoryEntry blah = AD.GetDirectoryEntry("LDAP://" + server + "/" + userPrincipal.DistinguishedName, strLoginID, strLoginPassword);
                    Console.WriteLine(blah.Properties.Contains("msDS-ReplAttributeMetaData"));
                    //Console.WriteLine(blah.Properties["msDS-ReplAttributeMetaData"][0]);
                    //msDS-ReplAttributeMetaData

                    foreach (var name in result.Properties.PropertyNames)
                    {
                        Console.WriteLine(name.ToString());
                    }
                }
                

            }



            return ld;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            this.Text = Application.ProductName;
            //prepopulate the domain list
            FillDomainList();
        }

        private void FillDomainList()
        {
            comboBoxDomain.Items.Clear();

            
            try
            {
                //get the current domain
                Domain dom = Domain.GetCurrentDomain();
                string currentDomain;
                currentDomain= dom.Name;
                if (!string.IsNullOrEmpty(currentDomain))
                {
                    //add trusted domains
                    TrustRelationshipInformationCollection colTrusts = dom.GetAllTrustRelationships();
                    foreach (TrustRelationshipInformation trustInfo in colTrusts)
                    {
                        comboBoxDomain.Items.Add(trustInfo.TargetName);
                    }

                    //add current domain
                    comboBoxDomain.Items.Add(currentDomain);
                    comboBoxDomain.SelectedItem = currentDomain;
                }

                FillSites();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {

            unlockToolStripMenuItem.Visible = true;

            
            if (listView1.SelectedIndices.Count == 0)
            {
                e.Cancel = true;
                return;
            }

            int intSelectedIndex = listView1.SelectedIndices[0];

            int colIndexServer = GetColumnIndex(listView1, "Server Full Name");
            int colIndexUserState = GetColumnIndex(listView1, "User State");
            string user_id = textBoxUserIDSearch.Text;
            string user_state = listView1.Items[intSelectedIndex].SubItems[colIndexUserState].Text;
            string server = listView1.Items[intSelectedIndex].SubItems[colIndexServer].Text;

            if (user_state!= "Locked")
            {
                unlockToolStripMenuItem.Visible = false;
                //e.Cancel = true;
                //return;
            }



        }

        private int GetColumnIndex(ListView lvi, string strColumnTitle)
        {
            foreach (ColumnHeader col in lvi.Columns)
            {
                if (col.Text == strColumnTitle)
                {
                    return col.Index;
                }
            }
            return 9999;
        }

        private void unlockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string strLoginID = textBoxUsername.Text;
            string strLoginPassword = textBoxPassword.Text;

            int intSelectedIndex = listView1.SelectedIndices[0];
            int colIndexServer = GetColumnIndex(listView1, "Server Full Name");
            int colUserState = GetColumnIndex(listView1, "User State");
            int colUserId = GetColumnIndex(listView1, "User ID");

            string server = listView1.Items[intSelectedIndex].SubItems[colIndexServer].Text;
            string username = listView1.Items[intSelectedIndex].SubItems[colUserId].Text; ;
                        
            PrincipalContext principalContext = AD.GetPrincipalContext(server,strLoginID,strLoginPassword);
            UserPrincipal userPrincipal = UserPrincipal.FindByIdentity(principalContext, username);

            userPrincipal.UnlockAccount();

            if (userPrincipal.IsAccountLockedOut())
            {
                MessageBox.Show("Account unlock failed.", "Unlock account", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            } 
            else
            {
                MessageBox.Show("Account unlocked successfully.", "Unlock account", MessageBoxButtons.OK, MessageBoxIcon.Information);
                listView1.Items[intSelectedIndex].SubItems[colUserState].Text = "Not Locked";
            }
             

        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 aboot = new AboutBox1();
            aboot.ShowDialog();
        }

        private void passwordResetToolStripMenuItem_Click(object sender, EventArgs e)
        {

            string strLoginID = textBoxUsername.Text;
            string strLoginPassword = textBoxPassword.Text;

            int intSelectedIndex = listView1.SelectedIndices[0];
            int colIndexServer = GetColumnIndex(listView1, "Server Full Name");
            int colUserId = GetColumnIndex(listView1, "User ID");

            string server = listView1.Items[intSelectedIndex].SubItems[colIndexServer].Text;
            string username = listView1.Items[intSelectedIndex].SubItems[colUserId].Text; ;

            PrincipalContext principalContext = AD.GetPrincipalContext(server,strLoginID,strLoginPassword);
            UserPrincipal userPrincipal = UserPrincipal.FindByIdentity(principalContext, username);

            using (PasswordReset passDlg = new PasswordReset())
            {
                passDlg.UserID = username;
                DialogResult result = passDlg.ShowDialog();
                
                if (result == DialogResult.OK)
                {
                    try
                    {
                        userPrincipal.SetPassword(passDlg.NewPassword);
                        if (passDlg.MustChange)
                        {
                            userPrincipal.ExpirePasswordNow();
                        }                        
                        MessageBox.Show("Password reset successfully.","Password reset",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Password reset failed.\n\n"+ex.Message, "Password reset", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    
                    
                }
            }
                
        }

        private void buttonFindUser_Click(object sender, EventArgs e)
        {
            using (UserSearch search = new UserSearch())
            {
                search.strServer = comboBoxDomain.Text;
                search.strLoginID = textBoxUsername.Text;
                search.strLoginPassword = textBoxPassword.Text;

                DialogResult result = search.ShowDialog();
                if (result == DialogResult.OK) {
                    textBoxUserIDSearch.Text = search.selectedUserId;
                }
            }


        }
    }


    
}
