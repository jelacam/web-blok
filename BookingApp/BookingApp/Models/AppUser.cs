using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookingApp.Models
{
    public class AppUser
    {
      
        public int Id { get; set; }
        public int FullName { get; set; }

        public List<RoomReservations> RoomReservations { get; set; }

        public List<Comment> Comment { get; set; }

        public List<Accommodation> Accommodation { get; set; }

    }
}