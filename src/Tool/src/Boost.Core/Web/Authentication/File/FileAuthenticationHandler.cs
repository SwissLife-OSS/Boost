using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Boost.Core.Settings;
using Boost.Infrastructure;
using Boost.Security;
using Boost.Settings;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Boost.Web.Authentication
{
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
                    AuthenticationTicket ticket = _ticketSerializer.Deserialize(data);

                    return AuthenticateResult.Success(ticket);
                }
                catch ( Exception ex)
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
        }

        private string GetTicketPath()
            => Path.Combine(SettingsStore.GetUserDirectory("auth"), Options.Filename);

        protected override Task HandleSignOutAsync(AuthenticationProperties? properties)
        {
            throw new NotImplementedException();
        }
    }
}
