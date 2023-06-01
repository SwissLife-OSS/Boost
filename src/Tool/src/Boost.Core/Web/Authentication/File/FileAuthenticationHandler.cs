using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Azure.Core;
using Boost.Core.Settings;
using Boost.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Boost.Web.Authentication;

public class FileAuthenticationHandler : SignInAuthenticationHandler<FileAuthenticationOptions>
{
    public FileAuthenticationHandler(
        IOptionsMonitor<FileAuthenticationOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock,
        IDataSerializer<AuthenticationTicket> ticketSerializer,
        IAuthTokenStore authTokenStore)
        : base(options, logger, encoder, clock)
    {
        _ticketSerializer = ticketSerializer;
        _authTokenStore = authTokenStore;
    }

    private Task<AuthenticateResult>? _readFileTask;
    private readonly IDataSerializer<AuthenticationTicket> _ticketSerializer;
    private readonly IAuthTokenStore _authTokenStore;

    private async Task<AuthenticateResult> ReadFileTicket()
    {
        var path = GetTicketPath();
        if (File.Exists(path))
        {
            try
            {
                byte[] data = await File.ReadAllBytesAsync(path);
                AuthenticationTicket? ticket = _ticketSerializer.Deserialize(data);

                if (ticket is { })
                {
                    DateTimeOffset? expiresUtc = GetExpiresAt(ticket);

                    if (expiresUtc is { } && expiresUtc.Value < Clock.UtcNow)
                    {
                        File.Delete(path);
                        return AuthenticateResult.Fail("Ticket expired");
                    }

                    return AuthenticateResult.Success(ticket);
                }

                return AuthenticateResult.Fail($"No ticket");
            }
            catch (Exception ex)
            {
                AuthenticateResult.Fail(ex);
            }
        }

        return AuthenticateResult.Fail($"No file: {path}");
    }

    private Task<AuthenticateResult> EnsureFileTicket()
    {
        if (_readFileTask == null)
        {
            _readFileTask = ReadFileTicket();
        }

        return _readFileTask;
    }

    protected async override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        AuthenticateResult? result = await EnsureFileTicket();
        if (!result.Succeeded)
        {
            return result;
        }

        return result;
    }

    protected override async Task HandleSignInAsync(
        ClaimsPrincipal user,
        AuthenticationProperties? properties)
    {
        var signInContext = new FileSigningInContext(
            Context,
            Scheme,
            Options,
            user,
            properties ?? new AuthenticationProperties());

        var ticket = new AuthenticationTicket(
            signInContext.Principal!,
            signInContext.Properties,
            signInContext.Scheme.Name);

        byte[] data = _ticketSerializer.Serialize(ticket);

        var path = GetTicketPath();
        await File.WriteAllBytesAsync(path, data);

        if (Options.SaveTokens)
        {
            TokenStoreModel model = CreateStoreModel(Options.Filename, ticket);

            await _authTokenStore.StoreAsync(model, Context.RequestAborted);
        }
    }

    private TokenStoreModel CreateStoreModel(
        string name,
        AuthenticationTicket ticket)
    {
        var model = new TokenStoreModel(name, Clock.UtcNow.UtcDateTime);

        if (name.StartsWith("R"))
        {
            model.RequestId = Guid.Parse(name.Split("-").Last());
        }

        if (ticket.Properties.Items.ContainsKey(".Token.expires_at") &&
            ticket.Properties.Items.ContainsKey(".Token.access_token"))
        {
            DateTimeOffset? expires = GetExpiresAt(ticket);

            var accessToken = ticket.Properties.Items[".Token.access_token"]!;
            model.Tokens.Add(new TokenInfo(TokenType.Access, accessToken)
            {
                ExpiresAt = expires
            });
        }

        if (ticket.Properties.Items.ContainsKey(".Token.id_token"))
        {
            model.Tokens.Add(new TokenInfo(
                TokenType.Id,
                ticket.Properties.Items[".Token.id_token"]!));
        }

        if (ticket.Properties.Items.ContainsKey(".Token.refresh_token"))
        {
            model.Tokens.Add(new TokenInfo(
                TokenType.Refresh,
                ticket.Properties.Items[".Token.refresh_token"]!));
        }

        return model;
    }

    private string GetTicketPath()
        => Path.Combine(SettingsStore.GetUserDirectory("auth"), Options.Filename);

    private DateTimeOffset? GetExpiresAt(AuthenticationTicket ticket)
    {
        if (ticket.Properties.ExpiresUtc.HasValue)
        {
            return ticket.Properties.ExpiresUtc;
        }

        DateTimeOffset? expires = null;
        if (ticket.Properties.Items.TryGetValue(".Token.expires_at", out var expiresAt) &&
            DateTime.TryParse(expiresAt, out DateTime parsed))
        {
            expires = parsed;
        }

        return expires;
    }

    protected override Task HandleSignOutAsync(AuthenticationProperties? properties)
    {
        var path = GetTicketPath();

        if (File.Exists(path))
        {
            File.Delete(path);
        }

        return Task.CompletedTask;
    }
}
