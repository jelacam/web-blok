using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Timers;
using System.Threading.Tasks;

namespace BookingApp.Hubs
{
    //[Authorize(Roles = "Admin, Manager")]
    [HubName("notifications")]
    public class NotificationHub : Hub
    {
        private static IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
        private static Timer t = new Timer();

        public void Hello()
        {
            Clients.All.hello("Hello from server");
        }

        public static void Notify(int clickCount)
        {
            //hubContext.Clients.Group("Admins").clickNotification($"Clicks: {clickCount}");

        }

        public static void SendNotification(int id)
        {
            hubContext.Clients.All.clickNotification(id);
        }

        public void GetTime()
        {
            Clients.All.setRealTime(DateTime.Now.ToString("h:mm:ss tt"));
        }

        public void TimeServerUpdates()
        {
            t.Interval = 1000;
            t.Start();
            t.Elapsed += OnTimedEvent;
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            GetTime();
        }

        public void StopTimeServerUpdates()
        {
            t.Stop();
        }

        public override Task OnConnected()
        {
            //Ako vam treba pojedinacni User
            //var identityName = Context.User.Identity.Name;

            //Groups.Add(Context.ConnectionId, "Admins");

            if (Context.User.IsInRole("Admin"))
            {
                Groups.Add(Context.ConnectionId, "Admins");
            }
            


            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            //Groups.Remove(Context.ConnectionId, "Admins");

            if (Context.User.IsInRole("Admin"))
            {
                Groups.Remove(Context.ConnectionId, "Admins");
            }

            return base.OnDisconnected(stopCalled);
        }
    }
}