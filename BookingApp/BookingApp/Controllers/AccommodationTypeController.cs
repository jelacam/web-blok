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
    [RoutePrefix("api/acctype")]
    public class AccommodationTypeController : ApiController
    {
        private BAContext db = new BAContext();

        [HttpGet]
        [Route("acctypes", Name = "AccType")]
        public IHttpActionResult GetAccTypes()
        {
            DbSet<AccommodationType> acctypes = db.AppAccommodationTypes;

            if(acctypes == null)
            {
                return NotFound();
            }

            return Ok(acctypes);
        }

        [HttpPut]
        [Route("acctypes/{id}")]
        public IHttpActionResult PutAccType(int id, AccommodationType acctype)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != acctype.Id)
            {
                return BadRequest("Ids are not matching!");
            }

            db.Entry(acctype).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (db.AppAccommodationTypes.Find(id) == null)
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

        [HttpDelete]
        [Route("acctypes/{id}")]
        public IHttpActionResult DeleteRegion(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            AccommodationType acctype = new AccommodationType();

            foreach (var item in db.AppAccommodationTypes)
            {
                if (id == item.Id)
                {
                    acctype = item;
                    break;
                }
            }

            if (acctype == null)
            {
                return BadRequest(ModelState);
            }

            db.AppAccommodationTypes.Remove(acctype);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (db.AppAccommodationTypes.Find(id) == null)
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
        [Route("acctypes")]
        [ResponseType(typeof(AccommodationType))]
        public IHttpActionResult PostAccType(AccommodationType acctype)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool acctypeExists = false;

            foreach (var item in db.AppAccommodationTypes)
            {
                if(item.Name.Equals(acctype.Name))
                {
                    acctypeExists = true;
                    break;
                }
            }

            if(acctypeExists == false)
            {
                db.AppAccommodationTypes.Add(acctype);
                db.SaveChanges();

                return CreatedAtRoute("CountryApi", new { id = acctype.Id }, acctype);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
