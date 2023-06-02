using System;
using Boost.Settings;

namespace Boost.AzureDevOps;

public class AzureDevOpsConnectedService : IConnectedService
{
    public string Type => AzureDevOpsConstants.ServiceTypeName;

    public Guid Id { get; set; }

    public AzureDevOpsConnectedService(
        Guid id,
        string name,
        string account,
        string defaultProject,
        string personalAccessToken)
    {
        Id = id;
        Name = name;
        Account = account;
        DefaultProject = defaultProject;
        PersonalAccessToken = personalAccessToken;
    }

    public string Name { get; set; }
    public string Account { get; set; }
    public string DefaultProject { get; set; }
    public string PersonalAccessToken { get; set; }

    public string? DefaultWorkRoot { get; set; }
}
