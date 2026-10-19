using VishKat.Domain;

namespace VishKat.Tests;

public class BeginnerRuleTests
{
    [Theory]
    [InlineData(5, false)]
    [InlineData(6, true)]
    [InlineData(10, true)]
    public void Scooter_UsesBaseRule(int simple, bool expected)
    {
        var scooter = new Scooter("S-1", "Самокат", simple, 0, 0.5);
        Assert.Equal(expected, scooter.is_for_beginners);
    }

    [Theory]
    [InlineData(5, false)]
    [InlineData(6, true)]
    public void Bike_UsesBaseRule(int simple, bool expected)
    {
        var bike = new Bike("B-1", "Велосипед", simple, 0);
        Assert.Equal(expected, bike.is_for_beginners);
    }

    [Theory]
    [InlineData(6, false)]
    [InlineData(7, false)]
    [InlineData(8, true)]
    public void ElectroBike_UsesStricterRule(int simple, bool expected)
    {
        var electroBike = new ElectroBike("E-1", "Электровелосипед", simple, 0, 1.0);
        Assert.Equal(expected, electroBike.is_for_beginners);
    }
}
