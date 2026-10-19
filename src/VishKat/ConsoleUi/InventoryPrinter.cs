using System.Globalization;
using VishKat.Domain;
using VishKat.Services;

namespace VishKat.ConsoleUi;

public sealed class InventoryPrinter
{
    private readonly TextWriter output;
    public InventoryPrinter(TextWriter output)
    {
        this.output = output ?? throw new ArgumentNullException(nameof(output));
    }
    public void PrintSection(string title)
    {
        output.WriteLine();
        output.WriteLine($" {title} ");
    }
    public void PrintMessage(string message) => output.WriteLine(message);
    public void PrintResult(IInventory item, CheckResult result)
    {
        var status = result.passed ? "[ПРИНЯТО]   " : "[ОТКЛОНЕНО] ";
        output.WriteLine($"{status}{Describe(item)}: {result.reason}");
    }
    public void PrintCounts(Inventory inventory)
    {
        output.WriteLine($"Всего единиц на балансе: {inventory.GetCount()}");
        output.WriteLine($"  техника: {inventory.GetTransportCount()}");
        output.WriteLine($"  вещи:    {inventory.GetItemCount()}");
        output.WriteLine("По видам:");
        foreach (var (kind, count) in inventory.GetCountByKind())
        {
            output.WriteLine($"  {kind}: {count}");
        }
    }
    public void PrintEnergy(Inventory inventory)
    {
        foreach (var item in inventory.GetAll())
        {
            if (item is IEnergyInventory energy)
            {
                output.WriteLine($"  {Describe(item)}: {FormatKwh(energy.charge_value)}");
            }
        }

        output.WriteLine($"Суммарное суточное потребление парка: {FormatKwh(inventory.GetTotalEnergy())}");
    }
    public void PrintForBeginners(Inventory inventory)
    {
        var beginnersVehicles = inventory.GetForBeginners();
        if (beginnersVehicles.Count == 0)
        {
            output.WriteLine("Подходящей для новичков техники нет.");
            return;
        }

        foreach (var v in beginnersVehicles)
        {
            output.WriteLine($"  {Describe(v)}, простота {v.simple}/{Transport.MaxSimple}");
        }
    }
    public void PrintAllId(Inventory inventory)
    {
        foreach (var item in inventory.GetAll())
        {
            output.WriteLine($"  {item.id,-10} | {item.kind,-16} | {item.name}");
        }
    }
    private static string Describe(IInventory item) => $"{item.kind} {item.id} «{item.name}»";
    private static string FormatKwh(double kwh) => kwh.ToString("0.##", CultureInfo.InvariantCulture) + " кВт·ч";
}
