// See https://aka.ms/new-console-template for more information
using System.Diagnostics;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Xml;
using System.Xml.Serialization;

namespace GreetingService.API.Client;
public static class Program
{
    private const string _baseAddress = "https://rafe-asp-app.azurewebsites.net";
    //private const string _baseAddress = "http://localhost:5091";

    private static HttpClient _httpClient = new();
    private const string _getGreetingsCommand = "get greetings";
    private const string _getGreetingCommand = "get greeting ";
    private const string _writeGreetingCommand = "write greeting ";
    private const string _updateGreetingCommand = "update greeting ";
    private const string _exportGreetingsCommand = "export greetings";
    private const string _repeatingCallsCommand = "repeat calls ";
    private static string _from = "Rafe";
    private static string _to = "Anyone";
    public static async Task Main(string[] args)
    {

        var authParam = Convert.ToBase64String(Encoding.UTF8.GetBytes("Rafe:summer2022"));
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authParam);

        _httpClient.BaseAddress = new Uri(_baseAddress);
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
            Console.WriteLine(_exportGreetingsCommand);
            Console.WriteLine($"{_repeatingCallsCommand} [count]");

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
            else if (command.Equals(_exportGreetingsCommand, StringComparison.OrdinalIgnoreCase))
            {
                await ExportGreetingsAsync();
            }
            else if (command.StartsWith(_repeatingCallsCommand))
            {
                var countPart = command.Replace(_repeatingCallsCommand, "");

                if (int.TryParse(countPart, out var count))
                {
                    await RepeatCallsAsync(count);
                }
                else
                {
                    Console.WriteLine($"Could not parse {countPart} as int");
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

    private static async Task<IEnumerable<Greeting>> GetGreetingsAsync()
    {
       var response = await _httpClient.GetAsync("/api/greeting");
        response.EnsureSuccessStatusCode();
        var greetingList = await response.Content.ReadAsStringAsync();
        var greetings = JsonSerializer.Deserialize<IEnumerable<Greeting>>(greetingList);

        foreach (var greeting in greetings)
        {
            Console.WriteLine($"[{greeting.id}] [{greeting.timestamp}] ({greeting.from} -> {greeting.to}) - {greeting.message}");
        }
        Console.WriteLine();

        return greetings;


    }

    private static async Task GetGreetingAsync(Guid id)
    {
        var totalGreetings = await _httpClient.GetAsync("/api/greeting");
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
            var response = await _httpClient.PostAsJsonAsync("/api/greeting", greeting);
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
            var response = await _httpClient.PutAsJsonAsync("/api/greeting", greeting);
            Console.WriteLine($"Updated greeting. Service responded with: {response.StatusCode}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Update greeting failed: {e.Message}\n");
        }

    }

    private static async Task ExportGreetingsAsync()
    {
        var response = await _httpClient.GetAsync("api/greeting");
        response.EnsureSuccessStatusCode();                                                 //throws exception if HTTP response status is not a success status
        var responseBody = await response.Content.ReadAsStringAsync();
        var greetings = JsonSerializer.Deserialize<List<Greeting>>(responseBody);

        var filename = "greetingExport.xml";
        var xmlWriterSettings = new XmlWriterSettings
        {
            Indent = true,
        };
        using var xmlWriter = XmlWriter.Create(filename, xmlWriterSettings);
        var serializer = new XmlSerializer(typeof(List<Greeting>));                             //this xml serializer does not support serializing interfaces, need to convert to a concrete class
        serializer.Serialize(xmlWriter, greetings);                                   //convert our greetings of type IEnumerable (interface) to List (concrete class)

        Console.WriteLine($"Exported {greetings.Count()} greetings to {filename}\n");
    }

    private static async Task RepeatCallsAsync(int count)
    {
        var greetings = await GetGreetingsAsync();
        var greeting = greetings.First();

        //init a jobs list
        var jobs = new List<int>();
        
        for (int i = 0; i < count; i++)
        {
            jobs.Add(i);
        }

        var stopwatch = Stopwatch.StartNew();           //use stopwatch to measure elapsed time just like a real world stopwatch

        //I cheat by running multiple calls in parallel for maximum throughput - we will be limited by our cpu, wifi, internet speeds
        //This is a bit advanced and the syntax is new with lamdas - don't worry if you don't understand all of it.
        //I always copy this from the internet and adapt to my needs
        //Running this in Visual Studio debugger is slow, try running .exe file directly from File Explorer or command line prompt
        await Parallel.ForEachAsync(jobs, new ParallelOptions { MaxDegreeOfParallelism = 50 }, async (job, token) =>
        {
            var start = stopwatch.ElapsedMilliseconds;
            var response = await _httpClient.GetAsync($"api/greeting/{greeting.id}");
            var end = stopwatch.ElapsedMilliseconds;

            Console.WriteLine($"Response: {response.StatusCode} - Call: {job} - latency: {end - start} ms - rate/s: {job / stopwatch.Elapsed.TotalSeconds}");
        });
    }
    }

