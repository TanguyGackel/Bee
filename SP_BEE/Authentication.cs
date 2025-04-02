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


    private Authentication()
    {

    }

    private static Authentication? _instance;

    internal static Authentication Instance => _instance ??= new Authentication();

    internal void fill(string usernamead, string passwordad, string domain, string adserver, int port)
    {
        _adusername = usernamead;
        _adpassword = passwordad;
        _addomain = domain;
        _adserver = adserver;
        _ldapport = port;
    }

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
        Console.WriteLine("ConnecAD");
        LdapConnection ldap = await ConnectionAD();
        
        string searchBase = "OU=users,OU=lambda,DC=bee,DC=bee";
        string ldap_filter = String.Format("(&(objectClass=user)(sAMAccountName={0}))", username);
        string[] attrs = ["DistinguishedName", "SamAccountName", "memberOf"];
        
        Console.WriteLine("SearchAsync");
        LdapSearchResults searchResult = (LdapSearchResults) await ldap.SearchAsync(searchBase, LdapConnection.ScopeSub, ldap_filter, attrs, false);

        LdapEntry nextEntry;

        try
        {
            Console.WriteLine("NextAsync");
            nextEntry = await searchResult.NextAsync();
        }
        catch(LdapException e) 
        {
            await Console.Error.WriteLineAsync("Error: " + e.LdapErrorMessage);
            throw;
        }

        Console.WriteLine("Get");

        LdapAttribute dn = nextEntry.Get("DistinguishedName");
        LdapAttribute sam = nextEntry.Get("samAccountName");
        LdapAttribute groups = nextEntry.Get("memberOf");

        List<string> groupuser = new List<string>();
        Console.WriteLine("GetGroup");

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
    
    public bool CheckGroup(string ms, List<string> usergroups)
    {
        string group = MSRegister.Instance.Register.First(t => t.type == ms).group;
        return usergroups.Contains(group);
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
    public static string ReadPassword()
    {
        string pwd = string.Empty;
        ConsoleKey key;

        do
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);
            key = keyInfo.Key;

            if (key == ConsoleKey.Backspace && pwd.Length > 0)
            {
                Console.Write("\b \b");
                pwd = pwd[0..^1];
            }
            else if (!char.IsControl(keyInfo.KeyChar))
            {
                Console.Write("*");
                pwd += keyInfo.KeyChar;
            }
        } while (key != ConsoleKey.Enter);
        Console.WriteLine();
        return pwd;
    }
    
}

public class User
{
    internal string sam;
    internal string dn;
    internal List<string> groups;
    
}