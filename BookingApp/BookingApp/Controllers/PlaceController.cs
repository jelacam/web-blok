using BookingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace BookingApp.Controllers
{
    [RoutePrefix("api")]
    public class PlaceController : ApiController
    {
        private BAContext db = new BAContext();

        [HttpGet]
        [Route("places")]
        public IQueryable<Place> Places()
        {
            return db.AppPlaces;
        }

        //[HttpPut]
        //[Route("place/{id}")]
        //[ResponseType(typeof(void))]
        //public IHttpActionResult InsertPlace(int id, Place place)
        //{

        //}
    }
}
