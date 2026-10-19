using VishKat.ConsoleUi;
using VishKat.Infrastructure;
using VishKat.Services;

Console.OutputEncoding = System.Text.Encoding.UTF8;

var container = new DIContainer();
container.RegisterInstance<TextWriter>(Console.Out);
container.RegisterSingleton<ICheck, ServiceCenter>();
container.RegisterSingleton<Inventory, Inventory>();
container.RegisterSingleton<InventoryPrinter, InventoryPrinter>();
container.Resolve<DemoScenario>().Run();

