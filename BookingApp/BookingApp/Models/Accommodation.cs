using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BookingApp.Models
{
    public class Accommodation
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Address { get; set; }

        public double AverageGrade { get; set; }

        public double Latitude { get; set; }

        public double Longitute { get; set; }

        public string ImageURL { get; set; }

        public bool Approved { get; set; }

        [ForeignKey("Place")]
        public int PlaceId { get; set; }

        public Place Place { get; set; }

        public List<Comment> Comment { get; set; }

        [ForeignKey("AccommodationType")]
        public int AccommodationTypeId { get; set; }

        public AccommodationType AccommodationType { get; set; }

        public List<Room> Room { get; set; }

        [ForeignKey("AppUser")]
        public int AppUserId { get; set; }

        public AppUser AppUser { get; set; }
    }
}