namespace VishKat.Domain;

public sealed class ElectroBike : Transport, IEnergyInventory
{
    public const int ElectroBeginnerSimple = 8;

    public ElectroBike(string id, string name, int simple, int wear_percent, double charge_value)
        : base(id, name, simple, wear_percent)
    {
        this.charge_value = CheckEnergy(charge_value);
    }

    public override string kind => "Электровелосипед";

    public double charge_value { get; }

    public override bool is_for_beginners => simple >= ElectroBeginnerSimple;
}
