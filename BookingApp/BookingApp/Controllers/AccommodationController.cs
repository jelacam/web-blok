﻿using BookingApp.Models;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.OData;

namespace BookingApp.Controllers
{
    [RoutePrefix("api/accommodation")]
    public class AccommodationController : ApiController
    {
        private BAContext db = new BAContext();
        public static int ClickCount { get; set; }

        [EnableQuery]
        [AllowAnonymous]
        [HttpGet]
        [Route("accommodations", Name = "AccommodationApi")]
        public IQueryable<Accommodation> GetAccommodations()
        {
            var accommodations = db.AppAccommodations;

            if (accommodations == null)
            {
                return null;
            }

            return accommodations as IQueryable<Accommodation>;
        }

        [EnableQuery]
        [AllowAnonymous]
        [HttpGet]
        [Route("accommodations/approved")]
        public IQueryable<Accommodation> GetAprovedAccommodation()
        {
            var accommodations = db.AppAccommodations.Where(p => p.Approved == true);
            //var accommodations = db.AppAccommodations;

            if (accommodations == null)
            {
                return null;
            }

            return accommodations as IQueryable<Accommodation>;
        }

        [AllowAnonymous]
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

        [Authorize(Roles = "Manager")]
        [HttpPut]
        [Route("accommodations/{id}")]
        public IHttpActionResult PutAccommodation(int id, Accommodation accommodation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            if (!user.appUserId.Equals(accommodation.AppUserId))
            {
                return BadRequest();
            }
            if (id != accommodation.Id)
            {
                return BadRequest("Ids are not matching!");
            }

            var accommodationOld = db.AppAccommodations.Find(id);
            accommodation.AverageGrade = accommodationOld.AverageGrade;

            if (accommodation.ImageURL == null)
            {
                accommodation.ImageURL = accommodationOld.ImageURL;
            }

            accommodationOld.AccommodationTypeId = accommodation.AccommodationTypeId;
            accommodationOld.Address = accommodation.Address;
            accommodationOld.Description = accommodation.Description;
            accommodationOld.Latitude = accommodation.Latitude;
            accommodationOld.Longitute = accommodation.Longitute;
            accommodationOld.PlaceId = accommodation.PlaceId;
            accommodationOld.Name = accommodation.Name;
            //db.Entry(accommodation).State = EntityState.Modified;

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

        [Authorize(Roles = "Manager")]
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
                if (item.Name.Equals(accommodation.Name)
                    && item.Address.Equals(accommodation.Address)
                    && item.PlaceId.Equals(accommodation.PlaceId))
                {
                    accommodationExists = true;
                    break;
                }
            }

            if (accommodationExists == false)
            {
                db.AppAccommodations.Add(accommodation);
                db.SaveChanges();

                var manager = db.AppUsers.Find(accommodation.AppUserId);
                if (manager != null)
                {
                    accommodation.AppUser = manager;
                }

                Hubs.NotificationHub.SendNotification(accommodation);

                return CreatedAtRoute("AccommodationApi", new { id = accommodation.Id }, accommodation);
            }
            else
            {
                return BadRequest();
            }
        }

        [Authorize(Roles = "Manager")]
        [HttpPost]
        [Route("upload")]
        public HttpResponseMessage UploadJsonFile()
        {
            //string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/UploadFile";
            //String RelativePath = "~/" + path.Replace(HttpContext.Current.Request.PhysicalApplicationPath, String.Empty);

            HttpResponseMessage response = new HttpResponseMessage();

            var abc = Request.Properties.Values;
            var httpRequser = HttpContext.Current.Request;
            var fileCount = httpRequser.Files.Count;

            if (fileCount > 0)
            {
                for (int i = 0; i < fileCount; i++)
                {
                    var postedFile = httpRequser.Files[i];
                    var filePath = HttpContext.Current.Server.MapPath("~/Content/" + postedFile.FileName);
                    postedFile.SaveAs(filePath);
                }
            }

            return response;
        }

        [Authorize(Roles = "Manager")]
        [HttpDelete]
        [Route("delete/{id}")]
        public IHttpActionResult DeleteAccommodation(int id)
        {
            var accommodation = db.AppAccommodations.Find(id);

            if (accommodation == null)
            {
                return BadRequest(ModelState);
            }

            var user = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            var accommodationV = db.AppAccommodations.Find(id);
            if (!user.appUserId.Equals(accommodationV.AppUserId))
            {
                return BadRequest();
            }
            db.AppAccommodations.Remove(accommodation);

            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("approve/{id}")]
        public IHttpActionResult ApproveAccommodation(int id)
        {
            var accommodation = db.AppAccommodations.Find(id);

            if (accommodation != null)
            {
                accommodation.Approved = true;

                db.SaveChanges();

                Place place = db.AppPlaces.Find(accommodation.PlaceId);
                accommodation.Place = place;

                Hubs.NotificationHub.SendApprovedAccommodationNotification(accommodation);
            }
            else
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}