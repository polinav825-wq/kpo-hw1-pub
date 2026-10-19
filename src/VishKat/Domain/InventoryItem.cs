namespace VishKat.Domain;

public abstract class InventoryItem : IInventory
{
    protected InventoryItem(string id, string name)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            throw new ArgumentException("Инвентарный номер обязателен.", nameof(id));
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Наименование обязательно.", nameof(name));
        }

        this.id = id.Trim();
        this.name = name.Trim();
    }

    public string id { get; }
    public string name { get; }
    public abstract string kind { get; }

    protected static double CheckEnergy(double charge_value)
    {
        if (double.IsNaN(charge_value) || charge_value < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(charge_value), charge_value, "Суточный расход энергии не может быть отрицательным.");
        }

        return charge_value;
    }
}
