using VishKat.Domain;
using VishKat.Services;
namespace VishKat.Tests.Fakes;

public sealed class FakeCheck : ICheck
{
    private readonly CheckResult result;
    public FakeCheck(CheckResult result)
    {
        this.result = result;
    }
    public List<Itransport> Checked { get; } = new();
    public static FakeCheck AlwaysAccept() => new(CheckResult.Accept("fake: ок"));
    public static FakeCheck AlwaysReject(string reason = "fake: отказ") => new(CheckResult.Reject(reason));
    public CheckResult Check(Itransport t)
    {
        Checked.Add(t);
        return result;
    }
}
