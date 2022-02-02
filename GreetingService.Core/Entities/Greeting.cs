namespace GreetingService.Core.Entities
{
    public class Greeting
    {
        public string Message { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public DateTime Timestamp { get; set; }= DateTime.Now;
        public Guid Id { get; set; } = Guid.NewGuid();

        public virtual string GetMessage()
        {
            return $"\n{Timestamp}:\n{Message}";
        }
    }
}
