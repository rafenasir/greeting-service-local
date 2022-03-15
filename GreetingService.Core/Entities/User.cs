using GreetingService.Core.Exceptions;
using GreetingService.Core.HelperFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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

        public UserApprovalStatus ApprovalStatus { get; set; }
        public string ApprovalStatusNote { get; set; }
        public string ApprovalCode { get; set; } = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)).Replace("/", "").Replace("?", "");      //This code should not be easily guessed. Also remove any / and ? characters to avoid disrupting url routing when calling api
        public DateTime ApprovalExpiry { get; set; } = DateTime.Now.AddDays(1);                                     //Must be approved within 1 day
    }

    public enum UserApprovalStatus
    {
        Approved = 0,
        Rejected = 1,
        Pending = 2,
    }



}

