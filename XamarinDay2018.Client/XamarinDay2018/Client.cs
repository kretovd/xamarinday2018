using System;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json;

namespace XamarinDay2018.Core
{
    public class Client
    {
        private readonly HubConnection _connection;
        private readonly IHubProxy _proxy;

        public event EventHandler<string> OnMessageReceived;

        public Client()
        {
            _connection = new HubConnection("https://xamarinday2018azure.azurewebsites.net/");
            _proxy = _connection.CreateHubProxy("hub");
        }

        public async Task Connect()
        {
            if (_connection != null && _connection.State == ConnectionState.Disconnected)
            {
                await _connection.Start();
            }
        }

        public async Task<bool> StartColorChanging()
        {
            if (_connection != null && _connection.State == ConnectionState.Connected)
            {
                _proxy.On<string>("Color", (message) =>
                    OnMessageReceived?.Invoke(this, message));

                return true;
            }

            if (_connection != null && _connection.State == ConnectionState.Disconnected)
            {
                await _connection.Start();
                return await StartColorChanging();
            }

            return false;
        }

        public void Disconnnect()
        {
            if (_connection != null && _connection.State != ConnectionState.Disconnected)
            {
                _connection.Stop();
            }
        }
    }
}
