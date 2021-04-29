using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRTut.Hubs
{
    public class MyHub : Hub
    {
        public void Announce(string message)
        {
            Console.WriteLine(message);
            Clients.All.SendAsync("ReceiveMessage", message);
        }
    }
}
