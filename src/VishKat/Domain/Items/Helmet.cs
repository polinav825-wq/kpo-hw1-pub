namespace VishKat.Domain;

public sealed class Helmet : Equipment
{
    public Helmet(string id, string name)
        : base(id, name)
    {
    }

    public override string kind => "Шлем";
}
