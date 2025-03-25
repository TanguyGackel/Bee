namespace MS_RD.Models;

internal class TestModel
{
    internal TestModel()
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