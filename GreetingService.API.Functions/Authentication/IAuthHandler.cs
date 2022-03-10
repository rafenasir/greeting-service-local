using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreetingService.API.Functions.Authentication
{
    public interface IAuthHandler
    {
        public Task<bool> IsAuthorizedAsync(HttpRequest req);
        public bool IsAuthorized(HttpRequest req);

    }
}
