using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.DirectoryServices.AccountManagement;
using System.Xml;
using System.Text.RegularExpressions;

namespace AD_Unlocker
{
    public partial class Form1 : Form
    {

        /*
         TO DO:  add context menu option to unlock             
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
        }

        [Flags()]
        public enum ADS_USER_FLAG_ENUM
        {
            ADS_UF_SCRIPT = 0X0001,
            ADS_UF_ACCOUNTDISABLE = 0X0002,
            ADS_UF_HOMEDIR_REQUIRED = 0X0008,
            ADS_UF_LOCKOUT = 0X0010,
            ADS_UF_PASSWD_NOTREQD = 0X0020,
            ADS_UF_PASSWD_CANT_CHANGE = 0X0040,
            ADS_UF_ENCRYPTED_TEXT_PASSWORD_ALLOWED = 0X0080,
            ADS_UF_TEMP_DUPLICATE_ACCOUNT = 0X0100,
            ADS_UF_NORMAL_ACCOUNT = 0X0200,
            ADS_UF_INTERDOMAIN_TRUST_ACCOUNT = 0X0800,
            ADS_UF_WORKSTATION_TRUST_ACCOUNT = 0X1000,
            ADS_UF_SERVER_TRUST_ACCOUNT = 0X2000,
            ADS_UF_DONT_EXPIRE_PASSWD = 0X10000,
            ADS_UF_MNS_LOGON_ACCOUNT = 0X20000,
            ADS_UF_SMARTCARD_REQUIRED = 0X40000,
            ADS_UF_TRUSTED_FOR_DELEGATION = 0X80000,
            ADS_UF_NOT_DELEGATED = 0X100000,
            ADS_UF_USE_DES_KEY_ONLY = 0x200000,
            ADS_UF_DONT_REQUIRE_PREAUTH = 0x400000,
            ADS_UF_PASSWORD_EXPIRED = 0x800000,
            ADS_UF_TRUSTED_TO_AUTHENTICATE_FOR_DELEGATION = 0x1000000
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
    


        private string GetDefaultNamingContext()
        {
            string strDomain = comboBoxDomain.Text;
            string dnc = null;
            if (!string.IsNullOrEmpty(strDomain))
            {
                DirectoryEntry ent = GetDirectoryEntry("LDAP://" + strDomain + "/RootDSE");
                if (ent.Properties.Contains("defaultNamingContext"))
                {
                    dnc = ent.Properties["defaultNamingContext"][0].ToString();
                }
            }

            return dnc;
        }

        private Forest GetForest()
        {
            Forest forest = null;

            DirectoryContext contextForest = GetContext(DirectoryContextType.Forest);

            try
            {
                forest = Forest.GetForest(contextForest);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error getting forest", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            return forest;
        }

        private DirectoryContext GetContext(DirectoryContextType contextType)
        {
            string strDomain = comboBoxDomain.Text;
            string strUsername = textBoxUsername.Text;
            string strPassword = textBoxPassword.Text;

            DirectoryContext cont= null;
            if (!string.IsNullOrEmpty(strDomain))
            {
                if (string.IsNullOrEmpty(strUsername))
                {
                    cont = new DirectoryContext(contextType, strDomain);
                }
                else
                {
                    cont = new DirectoryContext(contextType, strDomain, strUsername, strPassword);
                }
            }
            
            return cont;
        }

        private DirectoryEntry GetDirectoryEntry(string path)
        {

            
            string strDomain = comboBoxDomain.Text;
            string strUsername = textBoxUsername.Text;
            string strPassword = textBoxPassword.Text;


            DirectoryEntry entry = null;
            try
            {
                if (!string.IsNullOrEmpty(strDomain))
                {
                    if (string.IsNullOrEmpty(strUsername))
                    {
                        entry = new DirectoryEntry(path);
                    }
                    else
                    {
                        //entry = new DirectoryEntry(path, strUsername, strPassword);
                        entry = new DirectoryEntry(path, strUsername, strPassword, AuthenticationTypes.Secure);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            

            return entry;
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
            string strConfigurationNamingContext;


            string strSite = comboBoxSite.Text;


            if (!string.IsNullOrEmpty(strDomain))
            {
                DirectoryEntry ent = GetDirectoryEntry("LDAP://" + strDomain + "/RootDSE");
                if (ent == null)
                    return sites;

                try
                {
                    if (ent.Properties.Contains("configurationNamingContext"))
                    {
                        strConfigurationNamingContext = ent.Properties["configurationNamingContext"][0].ToString();
                        string strSitesContainer = "LDAP://" + strDomain + "/cn=Sites," + strConfigurationNamingContext;
                        Forest x = GetForest();
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
                


                ListViewItem lvi = new ListViewItem(DC.Name);
                lvi.SubItems.Add(dcShortName);
                lvi.SubItems.Add(DC.SiteName);
                lvi.SubItems.Add(data.user_state);
                lvi.SubItems.Add(data.bad_pwd_count.ToString());
                lvi.SubItems.Add(data.last_bad_pwd);
                lvi.SubItems.Add(data.pwd_last_set);
                lvi.SubItems.Add(data.lockout_time);
                lvi.SubItems.Add(data.orig_lock);


                listView1.Items.Add(lvi);
            }
            
        }

        private LockoutData GetLockoutData(string server)
        {
            LockoutData ld = new LockoutData();

            DirectoryEntry root = GetDirectoryEntry("LDAP://" + server);
            DirectorySearcher searcher = new DirectorySearcher(root);

            string strFilter = "(&(objectCategory=person)(objectClass=user)(sAMAccountName=" + textBoxUserIDSearch.Text + "))";

            searcher.PageSize = 1000;
            searcher.Filter = strFilter;
            searcher.PropertiesToLoad.Add("userAccountControl");
            searcher.PropertiesToLoad.Add("pwdLastSet");
            searcher.PropertiesToLoad.Add("msDS-ReplAttributeMetaData");

            SearchResult result = searcher.FindOne();

            if (result != null)
            {
                if (result.Properties.Contains("userAccountControl"))
                {
                    int userAccountControlValue = (int)result.Properties["userAccountControl"][0];
                    ADS_USER_FLAG_ENUM userAccountControl = (ADS_USER_FLAG_ENUM)userAccountControlValue;

                    ld.user_state = (userAccountControlValue & (int)ADS_USER_FLAG_ENUM.ADS_UF_LOCKOUT) == (int)ADS_USER_FLAG_ENUM.ADS_UF_LOCKOUT ? "Locked" : "Not Locked";
                }

                if (result.Properties.Contains("pwdLastSet"))
                {
                    long lngPasswordChanged = (long)result.Properties["pwdLastSet"][0];
                    ld.pwd_last_set = DateTime.FromFileTime(lngPasswordChanged).ToString();
                }


                //Console.WriteLine(xml);

                PrincipalContext principalContext = GetPrincipalContext(server);
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
                    foreach (string prop in result.Properties["msDS-ReplAttributeMetaData"])
                    {
                        if (prop.ToLower().Contains("lockouttime") && prop.ToLower().Contains("pszlastoriginatingdsadn"))
                        {
                            XmlDocument xmlDoc = new XmlDocument();
                            xmlDoc.LoadXml(prop);

                            Console.WriteLine("*****************");
                            Console.WriteLine(xmlDoc.SelectSingleNode("DS_REPL_ATTR_META_DATA/pszLastOriginatingDsaDN").InnerText);
                            Console.WriteLine(xmlDoc.SelectSingleNode("DS_REPL_ATTR_META_DATA/pszLastOriginatingDsaDN").Value);
                            Console.WriteLine("*****************");

                            string strPattern = "CN=NTDS Settings,CN=(?<servername>[^,]+),";
                            Regex objRegEx = new Regex(strPattern, RegexOptions.IgnoreCase);
                            MatchCollection colMatches = objRegEx.Matches(prop);
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
                ld.lockout_time = userPrincipal.AccountLockoutTime==null? "N/A" : ((DateTime)userPrincipal.AccountLockoutTime).ToLocalTime().ToString();
                ld.last_bad_pwd = userPrincipal.LastBadPasswordAttempt == null ? "None" : ((DateTime)userPrincipal.LastBadPasswordAttempt).ToLocalTime().ToString();


                DirectoryEntry blah = GetDirectoryEntry("LDAP://" + server + "/" + userPrincipal.DistinguishedName);
                Console.WriteLine(blah.Properties.Contains("msDS-ReplAttributeMetaData"));
                //Console.WriteLine(blah.Properties["msDS-ReplAttributeMetaData"][0]);
                //msDS-ReplAttributeMetaData

                foreach (var name in result.Properties.PropertyNames)
                {
                    Console.WriteLine(name.ToString());
                }

            }



            return ld;
        }

        private PrincipalContext GetPrincipalContext(string server)
        {
            string strUsername = textBoxUsername.Text;
            string strPassword = textBoxPassword.Text;

            PrincipalContext principalContext = null;
            if (!string.IsNullOrEmpty(server))
            {
                if (string.IsNullOrEmpty(strUsername))
                {
                    principalContext = new PrincipalContext(ContextType.Domain, server);
                }
                else
                {
                    principalContext = new PrincipalContext(ContextType.Domain, server, strUsername, strPassword);
                }
            }

            return principalContext;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
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
    }
}
