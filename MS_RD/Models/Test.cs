namespace MS_RD.Models;

internal class Test
{
    internal Test()
    {
        Nom = "";
        Type = TestType.None;
    }
    
    internal int Id;
    internal string Nom;
    internal TestType Type;
    internal bool Valide;
}

internal enum TestType
{
    None,
    Fonctionnel,
    Industriel
}