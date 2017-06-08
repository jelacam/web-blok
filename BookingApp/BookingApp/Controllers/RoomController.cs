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
    [RoutePrefix("api/room")]
    public class RoomController : ApiController
    {
        private BAContext db = new BAContext();

        [HttpGet]
        [Route("rooms", Name = "RoomApi")]
        public IHttpActionResult GetRooms()
        {
            DbSet<Room> rooms = db.AppRooms;

            if (rooms == null)
            {
                return NotFound();
            }

            return Ok(rooms);

        }

        [HttpPut]
        [Route("rooms/{id}")]
        public IHttpActionResult PutRoom(int id, Room room)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
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

        [HttpPost]
        [Route("rooms")]
        [ResponseType(typeof(Room))]
        public IHttpActionResult PostRoom(Room room)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
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
