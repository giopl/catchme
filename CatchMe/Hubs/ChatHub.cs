using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using CatchMe.Helpers;

namespace CatchMe
{
    public class ChatHub : Hub
    {

        public void Send(string name, string message)
        {
            // Call the addNewMessageToPage method to update clients.

            
            Clients.All.addNewMessageToPage(name, string.Format("{0} users are connected", MyUsers.Count()));
            var x = Context.ConnectionId;
            //Clients.Client("lea").addNewMessageToPage(name, message);
        }


        public static ConcurrentDictionary<string, MyUserType> MyUsers = new ConcurrentDictionary<string, MyUserType>();

        public override Task OnConnected()
        {
            string cookievalue = string.Empty;
            if (Context.Request.Cookies["CM"] != null)
            {
                cookievalue = Context.Request.Cookies["CM"].Value;
            }

            MyUsers.TryAdd(Context.ConnectionId, new MyUserType() { name = cookievalue  ,ConnectionId = Context.ConnectionId });
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            MyUserType garbage;

            MyUsers.TryRemove(Context.ConnectionId, out garbage);

            return base.OnDisconnected(true);
        }

        public class MyUserType
        {
            public string ConnectionId { get; set; }
            // Can have whatever you want here
            public string name { get; set; }
        }

    }
}