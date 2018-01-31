using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace XamarinDay2018.Core
{
    public class Client
    {
        private readonly HubConnection _connection;

        public event EventHandler<string> OnMessageReceived;

        public Client()
        {
            _connection = new HubConnectionBuilder()
                .WithUrl("https://xamarinday2018azure.azurewebsites.net/hub")
                .WithTransport(Microsoft.AspNetCore.Sockets.TransportType.LongPolling)
                .WithConsoleLogger()
                .Build();
        }

        public async Task StartColorChanging()
        {
            await _connection.StartAsync();
            _connection.On<string>("Color", (message) =>
                OnMessageReceived?.Invoke(this, message));
        }

        public async void Disconnnect()
        {
            await _connection?.DisposeAsync();
        }
    }
}