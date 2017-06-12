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
    [RoutePrefix("api/accommodation")]
    public class AccommodationController : ApiController
    {
        private BAContext db = new BAContext();

        [HttpGet]
        [Route("accommodations", Name = "AccommodationApi")]
        public IHttpActionResult GetAccommodations()
        {
            DbSet<Accommodation> accommodations = db.AppAccommodations;

            if (accommodations == null)
            {
                return NotFound();
            }

            return Ok(accommodations);
        }

        [HttpGet]
        [Route("accommodation/{id}")]
        public IHttpActionResult GetAccommodations(int id)
        {
            BAContext db = new BAContext();


            Accommodation acmd = db.AppAccommodations.Find(id);

            if (acmd == null)
            {
                return NotFound();
            }

            return Ok(acmd);
        }


        [HttpPut]
        [Route("accommodations/{id}")]
        public IHttpActionResult PutAccommodation(int id, Accommodation accommodation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != accommodation.Id)
            {
                return BadRequest("Ids are not matching!");
            }

            db.Entry(accommodation).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (db.AppAccommodations.Find(id) == null)
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
        [Route("accommodations")]
        [ResponseType(typeof(Accommodation))]
        public IHttpActionResult PostAccommodation(Accommodation accommodation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool accommodationExists = false;
            foreach (var item in db.AppAccommodations)
            {
                if(item.Name.Equals(accommodation.Name)
                    && item.Address.Equals(accommodation.Address)
                    && item.Place.Equals(accommodation.Place))
                {
                    accommodationExists = true;
                    break;
                }

            }

            if (accommodationExists == false)
            {
                db.AppAccommodations.Add(accommodation);
                db.SaveChanges();

                return CreatedAtRoute("AccommodationApi", new { id = accommodation.Id }, accommodation);
            }
            else
            {
                return BadRequest();
            }
        }

    }
}
