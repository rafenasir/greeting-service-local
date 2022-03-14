using GreetingService.Core.Exceptions;
using GreetingService.Core.HelperFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreetingService.Core
{
    public class User
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        private string _email;
        public string Email { get
            { return _email; }
            set {
                if (!EmailValidator.IsValidEmail(value))
                    throw new InvalidEmailException($"{value} is not a proper email address");
                _email = value;
            } }
        public string Password { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

    }
}
