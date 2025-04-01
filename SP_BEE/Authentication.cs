using System.Net;
using System.Text.RegularExpressions;
using Novell.Directory.Ldap;
using Novell.Directory.Ldap.Utilclass;

namespace SP_BEE;

public class Authentication
{
    private string _adusername; // dn format
    private string _adpassword;
    private string _addomain;
    private string _adserver;
    private int _ldapport;


    public Authentication(string usernamead, string passwordad, string domain, string adserver, int port)
    {
        _adusername = usernamead;
        _adpassword = passwordad;
        _addomain = domain;
        _adserver = adserver;
        _ldapport = port;
        // _password = password;
        // _username = username;
    }

    // public DirectoryEntry ConnectToAD()
    // {
    //     string ldappath = "LDAP://" + _addomain;
    //     DirectoryEntry ldapconnection;
    //     
    //     try
    //     {
    //         ldapconnection = new DirectoryEntry(ldappath, _adusername, _adpassword);
    //     }
    //     catch(Exception error)
    //     {
    //         Console.Error.WriteLine("Creation failed: ");
    //         Console.Error.WriteLine(error.Message);
    //         throw;
    //     }
    //
    //     return ldapconnection;
    //
    // }

    // public LdapConnection Connection()
    // {
    //     LdapDirectoryIdentifier ldapdomain = new LdapDirectoryIdentifier(_adserver, true, false);
    //     NetworkCredential creddomain = new NetworkCredential(_adusername, _adpassword, _addomain);
    //
    //     LdapConnection? ldapconnection = null;
    //
    //     try
    //     {
    //         ldapconnection = new LdapConnection(ldapdomain, creddomain, AuthType.Kerberos); // check the authType
    //     }
    //     catch (LdapException)
    //     {
    //         Console.Error.WriteLine("Failed to connect to AD server");
    //         throw;
    //     }
    //     catch (ArgumentException)
    //     {
    //         Console.Error.WriteLine("Invalid argument");
    //         throw;
    //     }
    //
    //     return ldapconnection;
    // }
    //
    // public void searchInOU(string userName)
    // {
    //     LdapConnection ldap = Connection();
    //
    //     string dn = "OU=users,OU=lambda,DC=bee,DC=bee";
    //     string ldap_filter = String.Format("(&(objectClass=user)(uid={0}))", userName);
    //
    //     SearchRequest search = new SearchRequest()
    //     {
    //         DistinguishedName = dn,
    //         Filter = ldap_filter,
    //         Scope = SearchScope.Subtree,
    //         Attributes = {"DistinguishedName"},
    //     };
    //
    // }

    public async Task<LdapConnection> ConnectionAD()
    {
        LdapConnection? conn = null;
        
        try
        {
            conn = new LdapConnection();
            await conn.ConnectAsync(_adserver, _ldapport); // adserver == ldap://hostname
            await conn.BindAsync(_adusername, _adpassword);
        }
        catch(LdapException e)
        {
            Console.WriteLine("Error:" + e.LdapErrorMessage);
            throw;
        }
        catch(Exception e)
        {
            Console.WriteLine("Error:" + e.Message);
            throw;
        }

        return conn;
    }

    public async Task<User> searchAD(string username)
    {
        LdapConnection ldap = await ConnectionAD();
        
        string searchBase = "OU=users,OU=lambda,DC=bee,DC=bee";
        string ldap_filter = String.Format("(&(objectClass=user)(sAMAccountName={0}))", username);
        string[] attrs = ["DistinguishedName", "SamAccountName", "memberOf"];

        LdapSearchResults searchResult = (LdapSearchResults) await ldap.SearchAsync(searchBase, LdapConnection.ScopeSub, ldap_filter, attrs, false);

        LdapEntry nextEntry;

        try
        {
            nextEntry = await searchResult.NextAsync();
        }
        catch(LdapException e) 
        {
            await Console.Error.WriteLineAsync("Error: " + e.LdapErrorMessage);
            throw;
        }


        LdapAttribute dn = nextEntry.Get("DistinguishedName");
        LdapAttribute sam = nextEntry.Get("samAccountName");
        LdapAttribute groups = nextEntry.Get("memberOf");

        List<string> groupuser = new List<string>();
        
        foreach (string value in groups.StringValueArray)
        {
            groupuser.Add(GetGroup(value));
        }

        User user = new User()
        {
            dn = dn.StringValue,
            sam = sam.StringValue,
            groups = groupuser,
        };

        return user;
        

    }
    
    string GetGroup(string value)
    {
        Match match = Regex.Match(value, "^CN=([^,]*)");

        if (!match.Success) return null;

        return match.Groups[1].Value;
    }
    
    public async Task<bool> AuthenticateUser(string username, string password)
    {
        User user = await searchAD(username);
       
        LdapConnection? conn = null;
        
        try
        {
            conn = new LdapConnection();
            await conn.ConnectAsync(_adserver, _ldapport); // adserver == ldap://hostname
            await conn.BindAsync(user.dn, password);
            return true;
        }
        catch(LdapException e)
        {
            Console.WriteLine("Error:" + e.LdapErrorMessage);
            return false;
        }
        catch(Exception e)
        {
            Console.WriteLine("Error:" + e.Message);
            return false;
        }

    }
    
}

public class User
{
    internal string sam;
    internal string dn;
    internal List<string> groups;
    
}