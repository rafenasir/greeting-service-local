using GreetingService.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace GreetingService.API.Functions.Authentication
{

    public class BasicAuthHandler : IAuthHandler
    {
        private readonly IUserService _userService;

        public BasicAuthHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<bool> IsAuthorizedAsync(HttpRequest req)
        {
            try
            {
                string authHeader = req.Headers["Authorization"];
                if (!string.IsNullOrWhiteSpace(authHeader))
                {
                    var authHeaderValue = AuthenticationHeaderValue.Parse(authHeader);
                    if (authHeaderValue.Scheme.Equals(AuthenticationSchemes.Basic.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        var credentials = Encoding.UTF8
                                            .GetString(Convert.FromBase64String(authHeaderValue.Parameter ?? string.Empty))         //decode base64encoded string to normal strings to parse username:password - don't confuse this with encryption, anybody with access to the encoded string can decode it like this
                                            .Split(':', 2);
                        if (credentials.Length == 2)
                        {
                            if (await _userService.IsValidUserAsync(credentials[0], credentials[1]))
                            {
                                return true;
                            }
                        }
                    }
                }

                return false;
            }
            catch (FormatException)
            {
                return false;
            }
        }

    }
}
