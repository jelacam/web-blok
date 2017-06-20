using BookingApp.Models;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace BookingApp.Hubs
{
    //[Authorize(Roles = "Admin, Manager")]
    [HubName("notifications")]
    public class NotificationHub : Hub
    {
        private static IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
        private static Timer t = new Timer();
        private static BAContext db = new BAContext();
        private static Dictionary<int, string> connections = new Dictionary<int, string>();

        public void Hello()
        {
            Clients.All.hello("Hello from server");
        }

        public static void Notify(int clickCount)
        {
            //hubContext.Clients.Group("Admins").clickNotification($"Clicks: {clickCount}");
        }

        /// <summary>
        ///     notifikacija admina da je kreiran smestaj
        /// </summary>
        /// <param name="id"></param>
        public static void SendNotification(Accommodation accommodation)
        {
            //hubContext.Clients.Group("Admins").clickNotification(accommodation);
            //List<Accommodation> accommodationToBeApproved = db.AppAccommodations.Where(p => p.Approved == false) as List<Accommodation>;

            hubContext.Clients.Group("Admins").clickNotification(accommodation);
        }


        public void GetNotification()
        {
            var accommodations = db.AppAccommodations.Where(p => p.Approved == false);

            if (accommodations != null)
            {
                foreach (var accom in accommodations)
                {
                    var manager = db.AppUsers.Find(accom.AppUserId);
                    if (manager != null)
                    {
                        accom.AppUser = manager;
                    }
                    hubContext.Clients.Group("Admins").clickNotification(accom);
                }
            }   

        }

        public static void SendApprovedAccommodationNotification(Accommodation accommodation)
        {
            hubContext.Clients.Group(accommodation.AppUserId.ToString()).recieveApprovedAccomodation(accommodation);
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

        public void RegisterForNotification(string userId, string userRole)
        {
            if (userRole.Equals("Admin"))
            {
                Groups.Add(Context.ConnectionId, "Admins");

            }
            else if (userRole.Equals("Manager"))
            {
                Groups.Add(Context.ConnectionId, userId);
            }
        }

        public void UnsubscribeForNotification(string userId, string userRole)
        {
            if (userRole.Equals("Admin"))
            {
                Groups.Remove(Context.ConnectionId, "Admins");
            }
            else if (userRole.Equals("Manager"))
            {
                Groups.Remove(Context.ConnectionId, userId);
            }
        }

        public override Task OnConnected()
        {
            //Ako vam treba pojedinacni User
            //var identityName = Context.User.Identity.Name;

            //Groups.Add(Context.ConnectionId, "Admins");

            //if (Context.User.IsInRole("Admin"))
            //{
            //    Groups.Add(Context.ConnectionId, "Admins");
            //}

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            //Groups.Remove(Context.ConnectionId, "Admins");

            //if (Context.User.IsInRole("Admin"))
            //{
            //    Groups.Remove(Context.ConnectionId, "Admins");
            //}

            return base.OnDisconnected(stopCalled);
        }
    }
}