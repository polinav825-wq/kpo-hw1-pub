using VishKat.Domain;

namespace VishKat.Services;

public interface ICheck
{
    CheckResult Check(Itransport t);
}
