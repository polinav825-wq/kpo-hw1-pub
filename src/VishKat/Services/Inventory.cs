using VishKat.Domain;

namespace VishKat.Services;

public sealed class Inventory
{
    private readonly List<IInventory> items = new();
    private readonly ICheck techCheck;
    public Inventory(ICheck check)
    {
        techCheck = check ?? throw new ArgumentNullException(nameof(check));
    }
    public CheckResult AddTransport(Itransport t)
    {
        ArgumentNullException.ThrowIfNull(t);
        if (HasId(t.id))
        {
            return DuplicateId(t.id);
        }
        var result = techCheck.Check(t);
        if (result.passed)
        {
            items.Add(t);
        }
        return result;
    }
    public CheckResult AddItem(Equipment item)
    {
        ArgumentNullException.ThrowIfNull(item);
        if (HasId(item.id))
        {
            return DuplicateId(item.id);
        }
        items.Add(item);
        return CheckResult.Accept("поставлено на баланс");
    }
    public IReadOnlyList<IInventory> GetAll() => items;
    public int GetCount() => items.Count;
    public int GetTransportCount() => items.OfType<Itransport>().Count();
    public int GetItemCount() => items.OfType<Equipment>().Count();
    public Dictionary<string, int> GetCountByKind() =>
        items.GroupBy(i => i.kind).ToDictionary(g => g.Key, g => g.Count());
    public double GetTotalEnergy() => items.OfType<IEnergyInventory>().Sum(e => e.charge_value);
    public List<Itransport> GetForBeginners() =>
        items.OfType<Itransport>().Where(t => t.is_for_beginners).ToList();
    public List<string> GetAllId() => items.Select(i => i.id).ToList();
    private bool HasId(string id) =>
        items.Any(i => string.Equals(i.id, id, StringComparison.OrdinalIgnoreCase));
    private static CheckResult DuplicateId(string id) =>
        CheckResult.Reject($"инвентарный номер {id} уже занят");
}
