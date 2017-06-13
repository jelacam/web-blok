using BookingApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace BookingApp.Controllers
{
    [RoutePrefix("api/place")]
    public class PlaceController : ApiController
    {
        private BAContext db = new BAContext();

        [HttpGet]
        [Route("places", Name = "PlaceApi")]
        public IHttpActionResult GetPlaces()
        {
            DbSet<Place> places = db.AppPlaces;

            if (places == null)
            {
                return NotFound();
            }

            return Ok(places);
     
        }

        [HttpPut]
        [Route("places/{id}")]
        public IHttpActionResult PutPlace(int id, Place place)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != place.Id)
            {
                return BadRequest("Ids are not matching!");
            }

            db.Entry(place).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (db.AppPlaces.Find(id) == null)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpPost]
        [Route("places")]
        [ResponseType(typeof(RoomReservations))]
        public IHttpActionResult PostPlace(Place place)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool placeExists = false;
            foreach (var item in db.AppRegions)
            {
                if (item.Name.Equals(place.Name) && item.Id.Equals(place.RegionId))
                {
                    placeExists = true;
                    break;
                }
            }

            if (placeExists == false)
            {
                db.AppPlaces.Add(place);
                db.SaveChanges();

                return CreatedAtRoute("CountryApi", new { id = place.Id }, place);
            }
            else
            {
                return BadRequest();
            }
        }

    }
}
