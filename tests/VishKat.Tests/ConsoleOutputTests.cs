using VishKat.ConsoleUi;
using VishKat.Domain;
using VishKat.Services;
using VishKat.Tests.Fakes;
namespace VishKat.Tests;

public class ConsoleOutputTests
{
    [Fact]
    public void PrintAllId_ListsTransportAndItemsWithNames()
    {
        var output = new StringWriter();
        var inventory = new Inventory(FakeCheck.AlwaysAccept());
        inventory.AddTransport(new Bike("B-1", "P", 9, 0));
        inventory.AddItem(new Helmet("H-1", "Шлем L"));

        new InventoryPrinter(output).PrintAllId(inventory);

        var text = output.ToString();
        Assert.Contains("B-1", text);
        Assert.Contains("P", text);
        Assert.Contains("H-1", text);
        Assert.Contains("Шлем L", text);
    }

    [Fact]
    public void PrintEnergy_ShowsTotal()
    {
        var output = new StringWriter();
        var inventory = new Inventory(FakeCheck.AlwaysAccept());
        inventory.AddTransport(new Scooter("S-1", "Самокат", 7, 0, 0.5));
        inventory.AddItem(new Staytion("D-1", "Станция", 2.4));

        new InventoryPrinter(output).PrintEnergy(inventory);

        Assert.Contains("Суммарное суточное потребление парка: 2.9 кВт·ч", output.ToString());
    }

    [Fact]
    public void PrintForBeginners_Empty_SaysNothingSuitable()
    {
        var output = new StringWriter();

        new InventoryPrinter(output).PrintForBeginners(new Inventory(FakeCheck.AlwaysAccept()));

        Assert.Contains("нет", output.ToString());
    }

    [Fact]
    public void DemoScenario_WithRealServiceCenter_RunsAllScenariosWithoutErrors()
    {
        var output = new StringWriter();
        var inventory = new Inventory(new ServiceCenter());

        new DemoScenario(inventory, new InventoryPrinter(output)).Run();

        var text = output.ToString();
        Assert.Contains("[ПРИНЯТО]", text);
        Assert.Contains("[ОТКЛОНЕНО]", text);
        Assert.Contains("[ОШИБКА]", text);
        Assert.Equal(5, inventory.GetTransportCount());
        Assert.Equal(4, inventory.GetItemCount());
        Assert.Equal(7.5, inventory.GetTotalEnergy(), precision: 6);
    }
}
