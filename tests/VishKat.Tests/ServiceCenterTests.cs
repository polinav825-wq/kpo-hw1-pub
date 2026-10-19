using VishKat.Domain;
using VishKat.Services;

namespace VishKat.Tests;

public class ServiceCenterTests
{
    private readonly ServiceCenter serviceCenter = new();

    [Fact]
    public void Check_GoodScooter_Passed()
    {
        var result = serviceCenter.Check(new Scooter("S-1", "Самокат", 7, wear_percent: 10, charge_value: 0.5));

        Assert.True(result.passed);
    }

    [Theory]
    [InlineData(30, true)]
    [InlineData(31, false)]
    [InlineData(80, false)]
    public void Check_WearLimit(int wear_percent, bool expected)
    {
        var result = serviceCenter.Check(new Bike("B-1", "Велосипед", 7, wear_percent));

        Assert.Equal(expected, result.passed);
    }

    [Theory]
    [InlineData(3.0, true)]
    [InlineData(3.1, false)]
    public void Check_EnergyLimit(double charge_value, bool expected)
    {
        var result = serviceCenter.Check(new ElectroBike("E-1", "Электровелосипед", 7, 0, charge_value));

        Assert.Equal(expected, result.passed);
    }

    [Fact]
    public void Check_Failed_ReasonExplainsWhy()
    {
        var result = serviceCenter.Check(new Scooter("S-1", "Самокат", 7, wear_percent: 50, charge_value: 0.5));

        Assert.False(result.passed);
        Assert.Contains("износ", result.reason);
    }

    [Fact]
    public void Check_Null_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => serviceCenter.Check(null!));
    }
}
