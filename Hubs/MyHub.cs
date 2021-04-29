using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRTut.Hubs
{
    public class MyHub : Hub
    {
        public async Task Announce(string user,string message)
        {
            Console.WriteLine(user," ", message);
            
            await Clients.All.SendAsync("ReceiveMessage", user,message);
        }
    }
}
