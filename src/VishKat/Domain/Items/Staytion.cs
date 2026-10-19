namespace VishKat.Domain;

public sealed class Staytion : Equipment, IEnergyInventory
{
    public Staytion(string id, string name, double charge_value)
        : base(id, name)
    {
        this.charge_value = CheckEnergy(charge_value);
    }

    public override string kind => "Док-станция";

    public double charge_value { get; }
}
