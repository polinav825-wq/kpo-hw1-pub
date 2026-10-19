namespace VishKat.Domain;

public interface Itransport : IInventory
{
    int simple { get; }
    int wear_percent { get; }
    bool is_for_beginners { get; }
}
