namespace VishKat.Domain;

public abstract class Transport : InventoryItem, Itransport
{
    public const int MinSimple = 1;
    public const int MaxSimple = 10;
    public const int BeginnerSimple = 6;

    protected Transport(string id, string name, int simple, int wear_percent)
        : base(id, name)
    {
        if (simple < MinSimple || simple > MaxSimple)
        {
            throw new ArgumentOutOfRangeException(
                nameof(simple), simple, $"Простота для новичка должна быть от {MinSimple} до {MaxSimple}.");
        }

        if (wear_percent < 0 || wear_percent > 100)
        {
            throw new ArgumentOutOfRangeException(
                nameof(wear_percent), wear_percent, "Износ задаётся в процентах от 0 до 100.");
        }

        this.simple = simple;
        this.wear_percent = wear_percent;
    }

    public int simple { get; }
    public int wear_percent { get; }
    public virtual bool is_for_beginners => simple >= BeginnerSimple;
}
