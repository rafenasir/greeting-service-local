using Azure.Messaging.ServiceBus;
using GreetingService.Core.Enums;
using GreetingService.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GreetingService.Infrastructure.MessagingService
{
    public class ServiceBusMessagingService : IMessagingService
    {
        private readonly ServiceBusSender _serviceBusSender;

        public ServiceBusMessagingService(ServiceBusSender serviceBusSender)
        {
            _serviceBusSender = serviceBusSender;
        }
        public async Task SendAsync<T>(T message, MessagingServiceSubject subject)
        {
            var serviceBusMessage = new ServiceBusMessage(JsonSerializer.Serialize(message))
            {
                Subject = subject.ToString()
            };

            await _serviceBusSender.SendMessageAsync(serviceBusMessage);
        }
    }
}
