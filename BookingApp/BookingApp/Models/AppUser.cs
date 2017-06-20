using Newtonsoft.Json;
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
        
    
        public string FullName { get; set; }

  
        public string UserName { get; set; }

        public bool Allow { get; set; }

        public List<RoomReservations> RoomReservations { get; set; }

        public List<Comment> Comment { get; set; }
        
        [JsonIgnore]
        public List<Accommodation> Accommodation { get; set; }

    }
}