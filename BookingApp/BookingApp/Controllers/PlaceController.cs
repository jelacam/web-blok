﻿using BookingApp.Models;
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

        [AllowAnonymous]
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

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("places/{id}")]
        public IHttpActionResult DeleteRegion(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Place place = new Place();

            foreach (var item in db.AppPlaces)
            {
                if (id == item.Id)
                {
                    place = item;
                    break;
                }
            }

            if (place == null)
            {
                return BadRequest(ModelState);
            }

            db.AppPlaces.Remove(place);

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

        [Authorize(Roles ="Admin")]
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
