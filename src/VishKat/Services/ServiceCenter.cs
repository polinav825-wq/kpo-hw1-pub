using System.Globalization;
using VishKat.Domain;
namespace VishKat.Services;

public sealed class ServiceCenter : ICheck
{
    public const int MaxWearPercent = 30;
    public const double MaxChargeValue = 3.0;
    public CheckResult Check(Itransport t)
    {
        ArgumentNullException.ThrowIfNull(t);
        if (t.wear_percent > MaxWearPercent)
        {
            return CheckResult.Reject($"износ {t.wear_percent}% выше допустимых {MaxWearPercent}%");
        }
        if (t is IEnergyInventory energy && energy.charge_value > MaxChargeValue)
        {
            return CheckResult.Reject(string.Format(
                CultureInfo.InvariantCulture,
                "высокий расход {0} кВт·ч/сутки (норма — до {1})",
                energy.charge_value,
                MaxChargeValue));
        }
        return CheckResult.Accept("техосмотр пройден");
    }
}
