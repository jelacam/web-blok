using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BookingApp.Models
{
    public class Place
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(40)]
        public string Name { get; set; }

        [ForeignKey("Region")]
        public int RegionId { get; set; }

        public Region Region { get; set; }

        [JsonIgnore]
        public List<Accommodation> Accomodation { get; set; }
    }
}