using BookingApp.Models;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.OData;

namespace BookingApp.Controllers
{
    [RoutePrefix("api/room")]
    public class RoomController : ApiController
    {
        private BAContext db = new BAContext();
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        [EnableQuery]
        [HttpGet]
        [Route("rooms", Name = "RoomApi")]
        public IQueryable<Room> GetRooms()
        {
            DbSet<Room> rooms = db.AppRooms;

            //List<Room> ret = new List<Room>();

            //foreach(var reservation in db.AppRoomReservations)
            //{
            //    foreach (var room in rooms)
            //    {
            //        if (room.Id != reservation.RoomId)
            //        {
            //            ret.Add(room);
            //        }
            //        else
            //        {

            //            string date = reservation.EndDate.Split('T')[0];
            //            DateTime endDate = DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            //            DateTime today = DateTime.Now;

            //            if (endDate < today)
            //            {
            //                ret.Add(room);
            //            }
            //        }
            //    }

               
            //}

            if (rooms == null)
            {
                return null;
            }

            return rooms;

        }

        [HttpGet]
        [Route("rooms/{id}")]
        public IHttpActionResult GetRooms(int id)
        {
            BAContext db = new BAContext();

            List<Room> rooms = new List<Room>(10);

            foreach (var room in db.AppRooms)
            {
                if (room.AccommodationId == id)
                {
                    rooms.Add(room);
                }
            }
 
            if (rooms == null)
            {
                return NotFound();
            }

            return Ok(rooms);

        }

        [Authorize(Roles = "Manager")]
        [HttpPut]
        [Route("rooms/{id}")]
        public IHttpActionResult PutRoom(int id, Room room)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            var accommodation = db.AppAccommodations.Find(room.AccommodationId);
            if (!user.appUserId.Equals(accommodation.AppUserId))
            {
                return BadRequest();
            }

            if (id != room.Id)
            {
                return BadRequest("Ids are not matching!");
            }

            db.Entry(room).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (db.AppRooms.Find(id) == null)
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
        [HttpDelete]
        [Route("rooms/{id}")]
        public IHttpActionResult DeleteRoom(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            var roomV = db.AppRooms.Find(id);
            var accommodation = db.AppAccommodations.Find(roomV.AccommodationId);
            if (!user.appUserId.Equals(accommodation.AppUserId))
            {
                return BadRequest();
            }

            Room room = new Room();

            foreach (var item in db.AppRooms)
            {
                if (id == item.Id)
                {
                    room = item;
                    break;
                }
            }

            if (room == null)
            {
                return BadRequest(ModelState);
            }

            db.AppRooms.Remove(room);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (db.AppRooms.Find(id) == null)
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
        [Route("rooms")]
        [ResponseType(typeof(Room))]
        public IHttpActionResult PostRoom(Room room)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            var accommodation = db.AppAccommodations.Find(room.AccommodationId);
            if (!user.appUserId.Equals(accommodation.AppUserId))
            {
                return BadRequest();
            }
            try
            {
                db.AppRooms.Add(room);

                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {

                throw;
            }

            return CreatedAtRoute("RoomApi", new { id = room.Id }, room);
        }
    }
}
