namespace VishKat.Domain;

public sealed class Scooter : Transport, IEnergyInventory
{
    public Scooter(string id, string name, int simple, int wear_percent, double charge_value)
        : base(id, name, simple, wear_percent)
    {
        this.charge_value = CheckEnergy(charge_value);
    }

    public override string kind => "Самокат";
    public double charge_value { get; }
}
