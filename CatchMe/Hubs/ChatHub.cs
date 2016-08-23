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
            var connectionIds = MyUsers.Where(x => x.Value.username == recipient).ToList();

            if(connectionIds.Count > 0)
            {
                foreach(var connectionId in connectionIds)
                context.Clients.Client(connectionId.Key).addNewMessageToPage("Admin", message);
            }
            else
            {
                //context.Clients.All.addNewMessageToPage("Admin", "this is a test message from the ihub");

            }

        }


        public void Send(string name, string message)
        {
            // Call the addNewMessageToPage method to update clients.


            var _message = MyUsers.Count() == 1 ? "1 user is currently connected" : string.Format("{0} users are currently connected", MyUsers.Count());
            Clients.All.addNewMessageToPage(name, _message);
            var x = Context.ConnectionId;
            //Clients.Client("lea").addNewMessageToPage(name, message);
        }


        public static ConcurrentDictionary<string, MyUserType> MyUsers = new ConcurrentDictionary<string, MyUserType>();

        public override Task OnConnected()
        {
            string cookievalue = string.Empty;
            string _username = string.Empty;
            string _browser = string.Empty;
            if (Context.Request.Cookies["CM"] != null)
            {
                cookievalue = Context.Request.Cookies["CM"].Value;

                var data = cookievalue.Split('|');
                _username = data[0];
                _browser = data[1];
            }

            MyUsers.TryAdd(Context.ConnectionId, new MyUserType() { name = cookievalue, username = _username  ,ConnectionId = Context.ConnectionId, connectionTime = DateTime.Now, browser = _browser });
            return base.OnConnected();
        }

        public override Task OnReconnected()
        {
               string cookievalue = string.Empty;
            string _username = string.Empty;
            string _browser = string.Empty;
            if (Context.Request.Cookies["CM"] != null)
            {
                cookievalue = Context.Request.Cookies["CM"].Value;

                var data = cookievalue.Split('|');
                _username = data[0];
                _browser = data[1];
            }

            MyUsers.TryAdd(Context.ConnectionId, new MyUserType() { name = cookievalue, username = _username  ,ConnectionId = Context.ConnectionId, connectionTime = DateTime.Now, browser = _browser });

            return base.OnReconnected();
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

            public DateTime connectionTime { get; set; }

        }

    }
}