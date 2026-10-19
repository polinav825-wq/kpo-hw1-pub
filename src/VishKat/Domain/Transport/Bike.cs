namespace VishKat.Domain;

public sealed class Bike : Transport
{
    public Bike(string id, string name, int simple, int wear_percent)
        : base(id, name, simple, wear_percent)
    {
    }

    public override string kind => "Велосипед";
}
