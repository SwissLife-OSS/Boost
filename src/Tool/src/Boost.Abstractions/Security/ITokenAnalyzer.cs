namespace Boost.Security;

public interface ITokenAnalyzer
{
    TokenModel? Analyze(string token);
}
