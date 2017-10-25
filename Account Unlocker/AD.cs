using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;




public class AD
{

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

    public static DirectoryEntry GetDirectoryEntry(string path, string username, string password)
    {


        DirectoryEntry entry = null;
        try
        {
            if (!string.IsNullOrEmpty(path))
            {
                if (string.IsNullOrEmpty(username))
                {
                    entry = new DirectoryEntry(path);
                }
                else
                {
                    //entry = new DirectoryEntry(path, username, password);
                    entry = new DirectoryEntry(path, username, password, AuthenticationTypes.Secure);
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }



        return entry;
    }



    public static PrincipalContext GetPrincipalContext(string server,string username,string password)
    {

        PrincipalContext principalContext = null;
        if (!string.IsNullOrEmpty(server))
        {
            if (string.IsNullOrEmpty(username))
            {
                principalContext = new PrincipalContext(ContextType.Domain, server);
            }
            else
            {
                principalContext = new PrincipalContext(ContextType.Domain, server, username, password);
            }
        }

        return principalContext;
    }


    public static string GetDefaultNamingContext(string domain,string username,string password)
    {


        string dnc = null;
        if (!string.IsNullOrEmpty(domain))
        {
            //DirectoryEntry ent = GetDirectoryEntry("LDAP://" + domain + "/RootDSE");
            DirectoryEntry ent = AD.GetDirectoryEntry("LDAP://" + domain + "/RootDSE", username, password);
            if (ent.Properties.Contains("defaultNamingContext"))
            {
                dnc = ent.Properties["defaultNamingContext"][0].ToString();
            }
        }

        return dnc;
    }



    public static Forest GetForest(string domain,string username,string password)
    {
        Forest forest = null;

        DirectoryContext contextForest = AD.GetContext(DirectoryContextType.Forest,domain,username,password);

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

    public static DirectoryContext GetContext(DirectoryContextType contextType,string domain,string username,string password)
    {


        DirectoryContext cont = null;
        if (!string.IsNullOrEmpty(domain))
        {
            if (string.IsNullOrEmpty(username))
            {
                cont = new DirectoryContext(contextType, domain);
            }
            else
            {
                cont = new DirectoryContext(contextType, domain, username, password);
            }
        }

        return cont;
    }

    public static SearchResultCollection GetSearchResults(string server, string filter, string[] properties, string strLoginID,string strLoginPassword)
    {
        SearchResultCollection results = null;
        try
        {
            DirectoryEntry root = GetDirectoryEntry("LDAP://" + server, strLoginID, strLoginPassword);
            DirectorySearcher searcher = new DirectorySearcher(root);

            searcher.PageSize = 1000;
            searcher.Filter = filter;
            foreach (string property in properties)
            {
                searcher.PropertiesToLoad.Add(property);
            }

            results = searcher.FindAll();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error getting results", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }


        return results;

    }

}

