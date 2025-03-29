namespace MS_Lib;

public static class Tools
{
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
    
    public static void ReadConfFile(Dictionary<string, string> conf, string path)
    {
        using StreamReader reader = new StreamReader(path);
        conf.Add("ipServer", reader.ReadLine());
        conf.Add("portServer", reader.ReadLine());
        conf.Add("dbConnectionType", reader.ReadLine());
        conf.Add("dbSource", reader.ReadLine());
        conf.Add("db", reader.ReadLine());
        conf.Add("clientNumber", reader.ReadLine());

        int.TryParse(conf["clientNumber"], out int n);
        for (int i = 0; i < n; i++)
        {
            conf.Add("clientConf" + i, reader.ReadLine());
        }
    }

    public static void ReadInput(IDictionary<string, string> conf)
    {
        Console.Write("Ip du serveur : ");
        conf.Add("ipServer", Console.ReadLine());
        Console.Write("Port du serveur : ");
        conf.Add("portServer", Console.ReadLine());
        Console.Write("Type de connection de la BDD :\n-Credentials\n-Windows Authentication\n");
        conf.Add("dbConnectionType", Console.ReadLine());
        Console.Write("Adresse de la BDD : ");
        conf.Add("dbSource", Console.ReadLine());
        Console.Write("Nom de la BDD : ");
        conf.Add("db", Console.ReadLine());
        Console.Write("Nombre de clients à contacter : ");
        conf.Add("clientNumber", Console.ReadLine());
        
        int.TryParse(conf["clientNumber"], out int n);
        for (int i = 0; i < n; i++)
        {
            Console.Write("Hostname/IP et port du client à contacter numero " + (i + 1) + " : \n");
            conf.Add("clientConf" + i, Console.ReadLine());
        }
    }
}