using VishKat.Domain;

namespace VishKat.Tests;

public class DomainValidationTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(11)]
    public void Transport_SimpleOutOfRange_Throws(int simple)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Bike("B-1", "Велосипед", simple, 0));
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(101)]
    public void Transport_WearOutOfRange_Throws(int wear_percent)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Bike("B-1", "Велосипед", 5, wear_percent));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Item_EmptyId_Throws(string id)
    {
        Assert.Throws<ArgumentException>(() => new Helmet(id, "Шлем"));
    }

    [Fact]
    public void Item_EmptyName_Throws()
    {
        Assert.Throws<ArgumentException>(() => new Helmet("H-1", " "));
    }

    [Fact]
    public void EnergyInventory_NegativeChargeValue_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Scooter("S-1", "Самокат", 5, 0, -0.1));
        Assert.Throws<ArgumentOutOfRangeException>(() => new ElectroBike("E-1", "Электровелосипед", 5, 0, -1));
        Assert.Throws<ArgumentOutOfRangeException>(() => new Staytion("D-1", "Станция", -1));
        Assert.Throws<ArgumentOutOfRangeException>(() => new ChargeCase("C-1", "Кейс", double.NaN));
    }

    [Fact]
    public void Item_TrimsIdAndName()
    {
        var helmet = new Helmet("  H-1 ", " Шлем L ");

        Assert.Equal("H-1", helmet.id);
        Assert.Equal("Шлем L", helmet.name);
    }

    [Fact]
    public void Kinds_AreDistinct()
    {
        IInventory[] all =
        {
            new Scooter("1", "a", 5, 0, 0),
            new Bike("2", "b", 5, 0),
            new ElectroBike("3", "c", 5, 0, 0),
            new Helmet("4", "d"),
            new Staytion("5", "e", 0),
            new ChargeCase("6", "f", 0),
        };

        Assert.Equal(all.Length, all.Select(i => i.kind).Distinct().Count());
    }
}
