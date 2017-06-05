using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BookingApp.Models
{
    public class RoomReservations
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public DateTime Timestamp { get; set; }

        [ForeignKey("Room")]
        public int RoomId { get; set; }

        public Room Room { get; set; }

        [ForeignKey("AppUser")]
        public int AppUserId { get; set; }

        public AppUser AppUser { get; set; }
    }
}