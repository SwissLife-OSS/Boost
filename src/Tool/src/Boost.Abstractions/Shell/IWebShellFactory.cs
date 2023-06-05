namespace Boost;

public interface IWebShellFactory
{
    IWebShell CreateShell(string shell);
    IWebShell CreateShell();
}
