namespace VishKat.Domain;

public abstract class Equipment : InventoryItem
{
    protected Equipment(string id, string name)
        : base(id, name)
    {
    }
}
