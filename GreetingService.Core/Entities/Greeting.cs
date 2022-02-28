using GreetingService.Core.Exceptions;
using GreetingService.Core.HelperFunctions;

namespace GreetingService.Core.Entities
{
    public class Greeting
    {
        public string Message { get; set; }
        private string _from;
        public string From
        {
            get
            {
                return _from;
            }
            set
            {
                if (!EmailValidator.IsValidEmail(value))               
                    throw new InvalidEmailException($"{value} is not a valid email address");
                _from = value;
            }
        }
        private string _to;
        public string To { get
            {
                return _to;
            }
            set {
                if (!EmailValidator.IsValidEmail(value))
                    throw new InvalidEmailException($"{value} is not a valid email address");
                _to = value;   
            } 
        }
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public Guid Id { get; set; } = Guid.NewGuid();

        public virtual string GetMessage()
        {
            return $"\n{Timestamp}:\n{Message}";
        }
    }
}
