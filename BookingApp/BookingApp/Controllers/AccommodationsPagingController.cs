using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.OData;
using System.Web.Http.OData.Routing;
using BookingApp.Models;

namespace BookingApp.Controllers
{
    /*
    The WebApiConfig class may require additional changes to add a route for this controller. Merge these statements into the Register method of the WebApiConfig class as applicable. Note that OData URLs are case sensitive.

    using System.Web.Http.OData.Builder;
    using System.Web.Http.OData.Extensions;
    using BookingApp.Models;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<Accommodation>("AccommodationsPaging");
    builder.EntitySet<AccommodationType>("AppAccommodationTypes"); 
    builder.EntitySet<AppUser>("AppUsers"); 
    builder.EntitySet<Comment>("AppComments"); 
    builder.EntitySet<RoomReservations>("AppRoomReservations"); 
    builder.EntitySet<Room>("AppRooms"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
   
    public class AccommodationsPagingController : ODataController
    {
        private BAContext db = new BAContext();

        // GET: odata/AccommodationsPaging
        [EnableQuery]
     
        public IQueryable<Accommodation> GetAccommodationsPaging()
        {
            DbSet<Accommodation> accommodations = db.AppAccommodations;

            if (accommodations == null)
            {
                return null;
            }

            return accommodations;
        }

        // GET: odata/AccommodationsPaging(5)
        [EnableQuery]
        public SingleResult<Accommodation> GetAccommodation([FromODataUri] int key)
        {
            return SingleResult.Create(db.AppAccommodations.Where(accommodation => accommodation.Id == key));
        }

        // PUT: odata/AccommodationsPaging(5)
        public IHttpActionResult Put([FromODataUri] int key, Delta<Accommodation> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Accommodation accommodation = db.AppAccommodations.Find(key);
            if (accommodation == null)
            {
                return NotFound();
            }

            patch.Put(accommodation);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccommodationExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(accommodation);
        }

        // POST: odata/AccommodationsPaging
        public IHttpActionResult Post(Accommodation accommodation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.AppAccommodations.Add(accommodation);
            db.SaveChanges();

            return Created(accommodation);
        }

        // PATCH: odata/AccommodationsPaging(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<Accommodation> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Accommodation accommodation = db.AppAccommodations.Find(key);
            if (accommodation == null)
            {
                return NotFound();
            }

            patch.Patch(accommodation);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccommodationExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(accommodation);
        }

        // DELETE: odata/AccommodationsPaging(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            Accommodation accommodation = db.AppAccommodations.Find(key);
            if (accommodation == null)
            {
                return NotFound();
            }

            db.AppAccommodations.Remove(accommodation);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/AccommodationsPaging(5)/AccommodationType
        [EnableQuery]
        public SingleResult<AccommodationType> GetAccommodationType([FromODataUri] int key)
        {
            return SingleResult.Create(db.AppAccommodations.Where(m => m.Id == key).Select(m => m.AccommodationType));
        }

        // GET: odata/AccommodationsPaging(5)/AppUser
        [EnableQuery]
        public SingleResult<AppUser> GetAppUser([FromODataUri] int key)
        {
            return SingleResult.Create(db.AppAccommodations.Where(m => m.Id == key).Select(m => m.AppUser));
        }

        // GET: odata/AccommodationsPaging(5)/Comment
        [EnableQuery]
        public IQueryable<Comment> GetComment([FromODataUri] int key)
        {
            return db.AppAccommodations.Where(m => m.Id == key).SelectMany(m => m.Comment);
        }

        // GET: odata/AccommodationsPaging(5)/Place
        [EnableQuery]
        public SingleResult<Place> GetPlace([FromODataUri] int key)
        {
            return SingleResult.Create(db.AppAccommodations.Where(m => m.Id == key).Select(m => m.Place));
        }

        // GET: odata/AccommodationsPaging(5)/Room
        [EnableQuery]
        public IQueryable<Room> GetRoom([FromODataUri] int key)
        {
            return db.AppAccommodations.Where(m => m.Id == key).SelectMany(m => m.Room);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AccommodationExists(int key)
        {
            return db.AppAccommodations.Count(e => e.Id == key) > 0;
        }
    }
}
