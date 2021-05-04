using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRTut.Hubs
{
    public class MyHub : Hub
    {
        private readonly OnlineUsers _onlineUsers;
        public MyHub(OnlineUsers users)
        {
            _onlineUsers = users;
            //_onlineUsers.CurrentUsers = new List<string>();
        }

        public async override Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "Public Room");
            await Clients.Caller.SendAsync("printConn",DateTime.Now);
        }

        public List<string> GetUsers()
        {
            Console.WriteLine();
            foreach(var item in _onlineUsers.CurrentUsers)
            {
                Console.WriteLine(item); 
            }
            return _onlineUsers.CurrentUsers;
        }

        public async override Task OnDisconnectedAsync(Exception? ex)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "Public Room");
            await Clients.Caller.SendAsync("printDisconn", DateTime.Now);

        }
        public async Task Announce(string user,string message)
        {
           // Console.WriteLine(user," ", message);
            if (!_onlineUsers.CurrentUsers.Contains(user.ToLower()))
            {
                _onlineUsers.CurrentUsers.Add(user);
                //Console.WriteLine(user);
            }
            await Clients.All.SendAsync("ReceiveMessage", user,message);
        }
    }
}
