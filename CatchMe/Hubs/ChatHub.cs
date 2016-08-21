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
        /// <summary>
        /// e
        /// </summary>
        /// <param name="fromFirstname"></param>
        /// <param name="touser"></param>
        /// <param name="toFirstname"></param>
        /// <param name="tasktitle"></param>
        public void SendNotification(string recipient, string message)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
            var connectionId = MyUsers.Where(x => x.Value.name == "GIO|Chrome 52.0").FirstOrDefault();

            if(connectionId.Key!=null)
            {
                context.Clients.Client(connectionId.Key).addNewMessageToPage("Admin", message);
            }
            else
            {
            context.Clients.All.addNewMessageToPage("Admin", "this is a test message from the ihub");

            }

        }


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
            string username = string.Empty;
            if (Context.Request.Cookies["CM"] != null)
            {
                cookievalue = Context.Request.Cookies["CM"].Value;

                var data = cookievalue.Split('|');
                username = data[0];
            }


            MyUsers.TryAdd(Context.ConnectionId, new MyUserType() { name = cookievalue, username = username  ,ConnectionId = Context.ConnectionId });
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

            public string username { get; set; }
            public string browser { get; set; }

        }

    }
}