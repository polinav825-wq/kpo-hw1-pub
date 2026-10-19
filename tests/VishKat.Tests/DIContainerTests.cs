using VishKat.Domain;
using VishKat.Infrastructure;
using VishKat.Services;
using VishKat.Tests.Fakes;

namespace VishKat.Tests;

public class DIContainerTests
{
    [Fact]
    public void Resolve_RegisteredInterface_ReturnsImplementation()
    {
        var container = new DIContainer();
        container.Register<ICheck, ServiceCenter>();

        Assert.IsType<ServiceCenter>(container.Resolve<ICheck>());
    }

    [Fact]
    public void Resolve_InjectsDependenciesIntoConstructor()
    {
        var container = new DIContainer();
        container.Register<ICheck, ServiceCenter>();

        var inventory = container.Resolve<Inventory>();

        Assert.NotNull(inventory);
    }

    [Fact]
    public void Resolve_Inventory_UsesSubstitutedCheck()
    {
        var fake = FakeCheck.AlwaysReject("fake отклонил");
        var container = new DIContainer();
        container.RegisterInstance<ICheck>(fake);
        container.RegisterSingleton<Inventory, Inventory>();
        var goodScooter = new Scooter("S-1", "Самокат", 10, wear_percent: 0, charge_value: 0.1);

        var result = container.Resolve<Inventory>().AddTransport(goodScooter);

        Assert.False(result.passed);
        Assert.Equal("fake отклонил", result.reason);
        Assert.Same(goodScooter, Assert.Single(fake.Checked));
    }

    [Fact]
    public void Resolve_Singleton_ReturnsSameInstance()
    {
        var container = new DIContainer();
        container.RegisterSingleton<ICheck, ServiceCenter>();

        Assert.Same(container.Resolve<ICheck>(), container.Resolve<ICheck>());
    }

    [Fact]
    public void Resolve_Transient_ReturnsNewInstanceEachTime()
    {
        var container = new DIContainer();
        container.Register<ICheck, ServiceCenter>();

        Assert.NotSame(container.Resolve<ICheck>(), container.Resolve<ICheck>());
    }

    [Fact]
    public void Resolve_UnregisteredInterface_Throws()
    {
        var container = new DIContainer();

        var exception = Assert.Throws<InvalidOperationException>(() => container.Resolve<ICheck>());
        Assert.Contains(nameof(ICheck), exception.Message);
    }

    [Fact]
    public void Resolve_PrimitiveDependency_ThrowsClearError()
    {
        var container = new DIContainer();

        Assert.Throws<InvalidOperationException>(() => container.Resolve<NeedsString>());
    }

    [Fact]
    public void Resolve_CyclicDependency_Throws()
    {
        var container = new DIContainer();

        Assert.Throws<InvalidOperationException>(() => container.Resolve<CycleA>());
    }

    [Fact]
    public void RegisterInstance_Null_Throws()
    {
        var container = new DIContainer();

        Assert.Throws<ArgumentNullException>(() => container.RegisterInstance<ICheck>(null!));
    }

    public sealed class NeedsString
    {
        public NeedsString(string value)
        {
            Value = value;
        }

        public string Value { get; }
    }

    public sealed class CycleA
    {
        public CycleA(CycleB other)
        {
        }
    }

    public sealed class CycleB
    {
        public CycleB(CycleA other)
        {
        }
    }
}
