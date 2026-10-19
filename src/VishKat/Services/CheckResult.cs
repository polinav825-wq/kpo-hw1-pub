namespace VishKat.Services;

public sealed record CheckResult(bool passed, string reason)
{
    public static CheckResult Accept(string reason = "принято") => new(true, reason);
    public static CheckResult Reject(string reason) => new(false, reason);
}
