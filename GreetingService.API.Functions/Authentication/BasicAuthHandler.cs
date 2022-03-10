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

        public bool IsAuthorized(HttpRequest req)
        {
            try
            {
                string authHeader = req.Headers["Authorization"];
                if (!string.IsNullOrWhiteSpace(authHeader))
                {
                    var authHeaderValue = AuthenticationHeaderValue.Parse(authHeader);
                    if(authHeaderValue.Scheme.Equals(AuthenticationSchemes.Basic.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        var credentials = Encoding.UTF8
                                          .GetString(Convert.FromBase64String(authHeaderValue.Parameter ?? String.Empty))
                                          .Split(':', 2);
                        if (credentials.Length == 2)
                        {
                            var isValid = _userService.IsValidUser(credentials[0], credentials[1]);
                            if (isValid)
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

        public Task<bool> IsAuthorizedAsync(HttpRequest req)
        {
            throw new NotImplementedException();
        }
    }
}
