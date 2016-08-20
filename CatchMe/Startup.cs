using Microsoft.Owin;
using Owin;
using CatchMe;

[assembly: OwinStartup(typeof(CatchMe.Startup))]
namespace CatchMe
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}