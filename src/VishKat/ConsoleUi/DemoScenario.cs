using VishKat.Domain;
using VishKat.Services;

namespace VishKat.ConsoleUi;

public sealed class DemoScenario
{
    private readonly Inventory inventory;
    private readonly InventoryPrinter printer;

    public DemoScenario(Inventory inventory, InventoryPrinter printer)
    {
        this.inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));
        this.printer = printer ?? throw new ArgumentNullException(nameof(printer));
    }

    public void Run()
    {
        printer.PrintMessage("Сервис ВышКат");

        AddTransport();
        ShowInvalidInput();
        AddItems();

        printer.PrintSection("4. Отчёт: число единиц");
        printer.PrintCounts(inventory);

        printer.PrintSection("5. Отчёт: суточное энергопотребление");
        printer.PrintEnergy(inventory);

        printer.PrintSection("6. Техника для новичков (простота ≥ 6, электровелосипед ≥ 8)");
        printer.PrintForBeginners(inventory);

        printer.PrintSection("7. Инвентарные номера: техника и вещи");
        printer.PrintAllId(inventory);

        printer.PrintMessage(string.Empty);
        printer.PrintMessage("Работа системы завершена");
    }

    private void AddTransport()
    {
        printer.PrintSection("1. Приём техники через техосмотр");

        var goodScooter = new Scooter("SC0", "A", simple: 8, wear_percent: 10, charge_value: 0.5);
        var hardScooter = new Scooter("SC1", "B", simple: 4, wear_percent: 12, charge_value: 0.6);
        var goodBike = new Bike("B0", "C", simple: 9, wear_percent: 5);
        var fastElectroBike = new ElectroBike("EB0", "D", simple: 7, wear_percent: 15, charge_value: 1.2);
        var easyElectroBike = new ElectroBike("EB1", "E", simple: 9, wear_percent: 20, charge_value: 1.0);

        var brokenScooter = new Scooter("SC2", "F", simple: 7, wear_percent: 45, charge_value: 0.5);
        var hungryElectroBike = new ElectroBike("EB2", "G", simple: 6, wear_percent: 5, charge_value: 4.5);

        var duplicateBike = new Bike("SC0", "H", simple: 10, wear_percent: 0);

        Itransport[] transport =
        {
            goodScooter, hardScooter, goodBike, fastElectroBike, easyElectroBike,
            brokenScooter, hungryElectroBike, duplicateBike,
        };

        foreach (var t in transport)
        {
            printer.PrintResult(t, inventory.AddTransport(t));
        }
    }

    private void ShowInvalidInput()
    {
        printer.PrintSection("2. Некорректные данные не попадают даже на техосмотр");

        TryCreate("простота 11", () => new Scooter("SC10", "scooter", simple: 11, wear_percent: 0, charge_value: 0.5));
        TryCreate("пустой номер", () => new Helmet("  ", "Шлем без номера"));
        TryCreate("отрицательный расход", () => new Staytion("ST", "Станция", charge_value: -1));
    }

    private void TryCreate(string caseName, Func<IInventory> create)
    {
        try
        {
            var item = create();
            printer.PrintMessage($"[?] {caseName}: объект создан ({item.id})");
        }
        catch (ArgumentException exception)
        {
            var reason = exception.Message.Split(" (")[0];
            printer.PrintMessage($"[ОШИБКА]    {caseName}: {reason}");
        }
    }

    private void AddItems()
    {
        printer.PrintSection("3. Постановка вещей на баланс");

        Equipment[] things =
        {
            new Helmet("HLM-S", "Шлем S"),
            new Helmet("HLM-L", "Шлем L"),
            new Staytion("ST-777", "Док-станция", charge_value: 2.4),
            new ChargeCase("CS0", "Зарядный кейс", charge_value: 1.8),
            new Helmet("HLM-S", "Шлем-дубликат"),
        };

        foreach (var thing in things)
        {
            printer.PrintResult(thing, inventory.AddItem(thing));
        }
    }
}
