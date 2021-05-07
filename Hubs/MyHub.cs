using Microsoft.AspNetCore.SignalR;
using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using System.IdentityModel;
using Microsoft.AspNetCore.Identity;
using SignalRTut.Data;
using SignalRTut.Models;

namespace SignalRTut.Hubs
{
    public class MyHub : Hub
    {
        private readonly OnlineUsers _onlineUsers;
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;

        public MyHub(OnlineUsers users,ApplicationDbContext db,UserManager<IdentityUser> userManager)
        {
            _onlineUsers = users;
            _db = db;
            _userManager = userManager;
        }

        public async override Task OnConnectedAsync()
        {
            //await Groups.AddToGroupAsync(Context.ConnectionId, "Public Room");
            _onlineUsers.MapUsers.Add(Context.User.Identity.Name,Context.ConnectionId);
            _onlineUsers.CurrentUsers.Add(Context.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            Console.WriteLine(_onlineUsers.MapUsers[Context.User.Identity.Name] + ":" + Context.User.Identity.Name);
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

        public async override Task OnDisconnectedAsync(Exception ex = null)
        {
            _onlineUsers.MapUsers.Remove(Context.User.Identity.Name);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "Public Room");
            _onlineUsers.CurrentUsers.Remove(Context.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            await Clients.Caller.SendAsync("printDisconn", DateTime.Now);

        }
        public async Task Announce(string user, string message)
        {
            string recId;
            try
            {
                recId = _onlineUsers.MapUsers[user];
                await Clients.Client(recId).SendAsync("ReceiveMessage", message);
               // var flag = recId != _onlineUsers.MapUsers[Context.User.Identity.Name];
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            var messageToDb = new Message();
            messageToDb.SenderId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            messageToDb.Text = message;
            messageToDb.RecId = _userManager.FindByEmailAsync(user).Result.Id;
            await _db.Messages.AddAsync(messageToDb);
            await _db.SaveChangesAsync();
        }
    }
}
