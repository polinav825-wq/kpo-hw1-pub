using VishKat.Domain;
using VishKat.Services;
using VishKat.Tests.Fakes;
namespace VishKat.Tests;

public class InventoryTests
{
    private static Scooter AnyScooter(string id = "S-1", int simple = 7, double charge_value = 0.5) =>
        new(id, "Самокат", simple, wear_percent: 10, charge_value: charge_value);

    [Fact]
    public void AddTransport_CheckPassed_TransportIsInInventory()
    {
        var check = FakeCheck.AlwaysAccept();
        var inventory = new Inventory(check);
        var goodScooter = AnyScooter();

        var result = inventory.AddTransport(goodScooter);

        Assert.True(result.passed);
        Assert.Contains(goodScooter, inventory.GetAll());
        Assert.Same(goodScooter, Assert.Single(check.Checked));
    }

    [Fact]
    public void AddTransport_CheckFailed_TransportIsNotInInventoryAndReasonReturned()
    {
        var inventory = new Inventory(FakeCheck.AlwaysReject("тормоза не работают"));

        var result = inventory.AddTransport(AnyScooter());

        Assert.False(result.passed);
        Assert.Equal("тормоза не работают", result.reason);
        Assert.Empty(inventory.GetAll());
    }

    [Fact]
    public void AddTransport_DuplicateId_RejectedWithoutSecondCheck()
    {
        var check = FakeCheck.AlwaysAccept();
        var inventory = new Inventory(check);
        inventory.AddTransport(AnyScooter("S-1"));

        var result = inventory.AddTransport(new Bike("s-1", "Велосипед", simple: 9, wear_percent: 0));

        Assert.False(result.passed);
        Assert.Contains("уже занят", result.reason);
        Assert.Single(check.Checked);
        Assert.Equal(1, inventory.GetCount());
    }

    [Fact]
    public void AddItem_DoesNotGoThroughCheck()
    {
        var check = FakeCheck.AlwaysReject();
        var inventory = new Inventory(check);

        var result = inventory.AddItem(new Helmet("H-1", "Шлем"));

        Assert.True(result.passed);
        Assert.Empty(check.Checked);
        Assert.Equal(1, inventory.GetItemCount());
    }

    [Fact]
    public void AddItem_IdTakenByTransport_Rejected()
    {
        var inventory = new Inventory(FakeCheck.AlwaysAccept());
        inventory.AddTransport(AnyScooter("X-1"));

        var result = inventory.AddItem(new Helmet("X-1", "Шлем"));

        Assert.False(result.passed);
        Assert.Equal(1, inventory.GetCount());
    }

    [Fact]
    public void Add_Null_Throws()
    {
        var inventory = new Inventory(FakeCheck.AlwaysAccept());

        Assert.Throws<ArgumentNullException>(() => inventory.AddTransport(null!));
        Assert.Throws<ArgumentNullException>(() => inventory.AddItem(null!));
    }

    [Fact]
    public void Constructor_NullCheck_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new Inventory(null!));
    }

    [Fact]
    public void GetTotalEnergy_CountsOnlyEnergyInventory()
    {
        var inventory = new Inventory(FakeCheck.AlwaysAccept());
        inventory.AddTransport(AnyScooter("S-1", charge_value: 0.5));
        inventory.AddTransport(new ElectroBike("E-1", "Электровелосипед", 9, 0, charge_value: 1.2));
        inventory.AddTransport(new Bike("B-1", "Велосипед", 9, 0));
        inventory.AddItem(new Helmet("H-1", "Шлем"));
        inventory.AddItem(new Staytion("D-1", "Станция", charge_value: 2.4));
        inventory.AddItem(new ChargeCase("C-1", "Кейс", charge_value: 1.8));

        Assert.Equal(5.9, inventory.GetTotalEnergy(), precision: 6);
    }

    [Fact]
    public void GetTotalEnergy_EmptyInventory_IsZero()
    {
        var inventory = new Inventory(FakeCheck.AlwaysAccept());

        Assert.Equal(0, inventory.GetTotalEnergy());
    }

    [Fact]
    public void GetTotalEnergy_RejectedTransportIsNotCounted()
    {
        var inventory = new Inventory(FakeCheck.AlwaysReject());
        inventory.AddTransport(AnyScooter(charge_value: 1.0));

        Assert.Equal(0, inventory.GetTotalEnergy());
    }

    [Fact]
    public void Counts_SplitTransportItemsAndKinds()
    {
        var inventory = new Inventory(FakeCheck.AlwaysAccept());
        inventory.AddTransport(AnyScooter("S-1"));
        inventory.AddTransport(AnyScooter("S-2"));
        inventory.AddTransport(new Bike("B-1", "Велосипед", 9, 0));
        inventory.AddItem(new Helmet("H-1", "Шлем"));

        Assert.Equal(4, inventory.GetCount());
        Assert.Equal(3, inventory.GetTransportCount());
        Assert.Equal(1, inventory.GetItemCount());

        var byKind = inventory.GetCountByKind();
        Assert.Equal(2, byKind["Самокат"]);
        Assert.Equal(1, byKind["Велосипед"]);
        Assert.Equal(1, byKind["Шлем"]);
    }

    [Fact]
    public void GetForBeginners_ReturnsOnlySuitableTransport()
    {
        var inventory = new Inventory(FakeCheck.AlwaysAccept());
        inventory.AddTransport(AnyScooter("S-1", simple: 6));
        inventory.AddTransport(AnyScooter("S-2", simple: 5));
        inventory.AddTransport(new ElectroBike("E-1", "Электровелосипед", simple: 7, wear_percent: 0, charge_value: 1));
        inventory.AddTransport(new ElectroBike("E-2", "Электровелосипед", simple: 8, wear_percent: 0, charge_value: 1));
        inventory.AddItem(new Helmet("H-1", "Шлем"));

        var beginnersVehicles = inventory.GetForBeginners();

        Assert.Equal(new[] { "S-1", "E-2" }, beginnersVehicles.Select(t => t.id));
    }

    [Fact]
    public void GetAllId_ContainsTransportAndItems()
    {
        var inventory = new Inventory(FakeCheck.AlwaysAccept());
        inventory.AddTransport(AnyScooter("S-1"));
        inventory.AddItem(new Helmet("H-1", "Шлем"));
        inventory.AddItem(new ChargeCase("C-1", "Кейс", 1.0));

        Assert.Equal(new[] { "S-1", "H-1", "C-1" }, inventory.GetAllId());
    }
}
