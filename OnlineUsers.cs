using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Threading.Tasks;

namespace SignalRTut
{
    public class OnlineUsers
    {
        public List<string> CurrentUsers = new List<string>();
        public Dictionary<string, string> MapUsers = new Dictionary<string, string>();
    }
}
