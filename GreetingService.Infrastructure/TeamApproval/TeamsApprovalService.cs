using GreetingService.Core;
using GreetingService.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreetingService.Infrastructure.TeamApproval
{
    public class TeamsApprovalService : IApprovalService
    {
		private readonly HttpClient _httpClient;
		private readonly string _teamsWebHookUrl;       //this url is generated when configuring a web hook connector to a Teams channel
		private readonly string _greetingServiceBaseUrl;
		private readonly ILogger<TeamsApprovalService> _logger;

		public TeamsApprovalService(HttpClient httpClient, IConfiguration configuration, ILogger<TeamsApprovalService> logger)
		{
			_httpClient = httpClient;
			_teamsWebHookUrl = configuration["TeamsWebHookUrl"];                            //remember to add this to application configuration (local.settings.json, bicep etc)
			_greetingServiceBaseUrl = configuration["GreetingServiceBaseUrl"];              //remember to add this to application configuration (local.settings.json, bicep etc)
			_logger = logger;
		}

		public async Task BeginUserApprovalAsync(User user)
        {
			var json = @$"{{
						""@type"": ""MessageCard"",
						""@context"": ""https://schema.org/extensions"",
						""summary"": ""Approval for new GreetingService user"",
						""sections"": [
							{{
									""title"": ""RAFE'S APPROVAL REQUEST"",
								""activityImage"": ""https://upload.wikimedia.org/wikipedia/commons/thumb/7/7c/User_font_awesome.svg/1024px-User_font_awesome.svg.png?20160212005950"",
								""activityTitle"": ""Approve new user in GreetingService: {user.Email}"",
								""activitySubtitle"": ""{user.FirstName} {user.LastName}"",
								""facts"": [
									{{
										""name"": ""Date submitted:"",
										""value"": ""{DateTime.Now:yyyy-MM-dd HH:mm}""
									}},
									{{
										""name"": ""Details:"",
										""value"": ""Please approve or reject the new user: {user.Email} for the GreetingService""
									}}
								]
							}},
							{{
								""potentialAction"": [
									{{
										""@type"": ""HttpPOST"",
										""name"": ""Approve"",
										""target"": ""{_greetingServiceBaseUrl}/api/user/approve/{user.ApprovalCode}""
	
									}},
									{{
										""@type"": ""HttpPOST"",
										""name"": ""Reject"",
										""target"": ""{_greetingServiceBaseUrl}/api/user/reject/{user.ApprovalCode}""
									}}
								]
							}}
						]
					}}";

			var response = await _httpClient.PostAsync(_teamsWebHookUrl, new StringContent(json));
			if (!response.IsSuccessStatusCode)
			{
				var responseBody = await response.Content?.ReadAsStringAsync();
				_logger.LogError("Failed to send approval to Teams for user {email}. Received this response body: {response}", user.Email, responseBody ?? "null");
			}

			response.EnsureSuccessStatusCode();
		}
    }
}
