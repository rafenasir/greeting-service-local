// See https://aka.ms/new-console-template for more information
using System.Net.Http.Json;
using System.Text.Json;

namespace GreetingService.API.Client;
public static class Program
{
    private const string RequestUri = "http://localhost:5091/api/greeting";
    private static HttpClient _httpClient = new();
    private const string _getGreetingsCommand = "get greetings";
    private const string _getGreetingCommand = "get greeting ";
    private const string _writeGreetingCommand = "write greeting ";
    private const string _updateGreetingCommand = "update greeting ";
    private static string _from = "Rafe";
    private static string _to = "Anyone";
    public static async Task Main(string[] args)
    {
        Console.WriteLine("Welcome to the Greeting Service Console Client");
        Console.WriteLine("Who is sending the Greeting");
        var from = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(from))
        {
            _from = from;
        }
        Console.WriteLine("Enter the Reciepient");
        var to = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(to))
        {
            _to = to;
        }

        while (true)
        {
            Console.WriteLine("Available Commands");
            Console.WriteLine(_getGreetingsCommand);
            Console.WriteLine($"{_getGreetingCommand} [id]");
            Console.WriteLine(_writeGreetingCommand);
            Console.WriteLine($"{_updateGreetingCommand} [id] [message]");

            Console.WriteLine("\nWrite command and press [enter] to execute");

            var command = Console.ReadLine();
            if (command == null)
            {
                Console.WriteLine("Please Enter the Command");
                continue;
            }
            if (command.Equals(_getGreetingsCommand, StringComparison.OrdinalIgnoreCase))
            {
                await GetGreetingsAsync();
            }
            else if (command.StartsWith(_getGreetingCommand,StringComparison.OrdinalIgnoreCase))
            {
                var greetingId = command.Replace(_getGreetingCommand, "");
                if(Guid.TryParse(greetingId, out var id))
                {
                    await GetGreetingAsync(id);
                }
                else { Console.WriteLine("Id you entered is not a valid Guid"); }
            }
            else if (command.StartsWith(_writeGreetingCommand, StringComparison.OrdinalIgnoreCase))
            {
                var message = command;
                await WriteGreetingAsync(message);
            }
            else if (command.StartsWith(_updateGreetingCommand, StringComparison.OrdinalIgnoreCase))
            {
                var completeCommand = command.Replace(_updateGreetingCommand, "") ?? "";
                var idPart = completeCommand.Split(" ").First();
                var message = completeCommand.Replace(idPart, "").Trim();

                if(Guid.TryParse(idPart, out var id))
                {
                    await UpdateGreetingAsync(id, message);
                }
                else
                {
                    Console.WriteLine($"{idPart} is not a valid Guid");
                }

            }
            else
            {
                Console.WriteLine("Command has not been recongnized");
            }
        }

        


        Console.WriteLine("Done");
        Console.ReadLine();
    }

    private static async Task GetGreetingsAsync()
    {
       var response = await _httpClient.GetAsync(RequestUri);
        var greetingList = await response.Content.ReadAsStringAsync();
        var greetings = JsonSerializer.Deserialize<IList<Greeting>>(greetingList);
        Console.WriteLine(greetings);
        foreach (var greeting in greetings)
        {
            Console.WriteLine(greeting.message);
        }

    }

    private static async Task GetGreetingAsync(Guid id)
    {
        var totalGreetings = await _httpClient.GetAsync(RequestUri);
        var greetingsString = await totalGreetings.Content.ReadAsStringAsync();
        var greetingsList = JsonSerializer.Deserialize<IList<Greeting>>(greetingsString);
        var finalGreeting =  greetingsList?.FirstOrDefault(x => x.id == id);
        Console.WriteLine(finalGreeting.message);   

    }

    private static async Task WriteGreetingAsync(string message)
    {
        try
        {
            var greeting = new Greeting
            {
                from = _from,
                to = _to,
                message = message,
            };
            var response = await _httpClient.PostAsJsonAsync(RequestUri, greeting);
            Console.WriteLine($"Wrote greeting. Service responded with: {response.StatusCode}");            //all HTTP responses always contain a status code
            Console.WriteLine();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Write greeting failed: {e.Message}\n");
        }
    }

    private static async Task UpdateGreetingAsync(Guid id, string message)
    {
        try
        {
            var greeting = new Greeting
            {
                id = id,
                from = _from,
                to = _to,
                message = message,
            };
            var response = await _httpClient.PutAsJsonAsync(RequestUri, greeting);
            Console.WriteLine($"Updated greeting. Service responded with: {response.StatusCode}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Update greeting failed: {e.Message}\n");
        }

    }
}

