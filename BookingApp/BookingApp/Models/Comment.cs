using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BookingApp.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

   
        public int Grade { get; set; }


        [MaxLength(400)]
        public string Text { get; set; }

   
        [ForeignKey("Accommodation")]
        public int AccommodationId { get; set; }

        public Accommodation Accommodation { get; set; }

  
        [ForeignKey("AppUser")]
        public int AppUserId { get; set; }

        public AppUser AppUser { get; set; }
    }
}