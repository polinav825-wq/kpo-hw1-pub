namespace VishKat.Domain;

public sealed class ChargeCase : Equipment, IEnergyInventory
{
    public ChargeCase(string id, string name, double charge_value)
        : base(id, name)
    {
        this.charge_value = CheckEnergy(charge_value);
    }

    public override string kind => "Зарядный кейс";

    public double charge_value { get; }
}
