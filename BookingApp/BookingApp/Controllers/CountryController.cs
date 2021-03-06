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
    [RoutePrefix("api/country")]
    public class CountryController : ApiController
    {
        private BAContext db = new BAContext();

        [AllowAnonymous]
        [HttpGet]
        [Route("countries", Name = "CountryApi")]
        public IHttpActionResult GetCountries()
        {
            DbSet<Country> countries = db.AppCountries;

            if (countries == null)
            {
                return NotFound();
            }

            return Ok(countries);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("countries/{id}")]
        public IHttpActionResult PutCountry(int id, Country country)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(id != country.Id)
            {
                return BadRequest("Ids are not matching!");
            }

            db.Entry(country).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (db.AppCountries.Find(id) == null)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(country);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("countries/{id}")]
        public IHttpActionResult DeleteCountry(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Country country = new Country();

            foreach (var item in db.AppCountries)
            {
                if(id == item.Id)
                {
                    country = item;
                    break;
                }
            }

            if(country == null)
            {
                return  BadRequest(ModelState);
            }

            db.AppCountries.Remove(country);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (db.AppCountries.Find(id) == null)
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
        [HttpPost]
        [Route("countries")]
        [ResponseType(typeof(Country))]
        public IHttpActionResult PostCountry(Country country)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool countryExists = false;
            foreach (var item in db.AppCountries)
            {
                if (item.Name.Equals(country.Name) && item.Code.Equals(country.Code))
                {
                    countryExists = true;
                    break;
                }
            }

            if(countryExists == false)
            {
                db.AppCountries.Add(country);
                db.SaveChanges();

                return CreatedAtRoute("CountryApi", new { id = country.Id }, country);
            }
            else
            {
                return BadRequest();
            }
            
        }

    }
}
