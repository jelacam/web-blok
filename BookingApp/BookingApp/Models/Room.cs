using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BookingApp.Models
{
    public class Room
    {
        [Key]
        public int Id { get; set; }

        public int RoomNumber { get; set; }

        public int BedCount { get; set; }

        [MaxLength(400)]
        public string Description { get; set; }

        public double PricePerNight { get; set; }

        [ForeignKey("Accommodation")]
        public int AccommodationId { get; set; }

        public Accommodation Accommodation { get; set; }

        public List<RoomReservations> RoomReservations { get; set; }

    }
}