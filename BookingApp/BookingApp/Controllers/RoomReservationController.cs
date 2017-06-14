﻿using BookingApp.Models;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;

namespace BookingApp.Controllers
{
    [RoutePrefix("api/RoomReservation")]
    public class RoomReservationController : ApiController
    {
        private BAContext db = new BAContext();
        public static object obj = new object();

        [HttpGet]
        [Route("roomReservations", Name = "RoomReservationApi")]
        public IHttpActionResult GetRoomReservations()
        {
            DbSet<RoomReservations> roomReservations = db.AppRoomReservations;

            if (roomReservations == null)
            {
                return NotFound();
            }

            return Ok(roomReservations);
        }

        [HttpPut]
        [Route("roomReservations/{id}")]
        public IHttpActionResult PutRoomReservations(int id, RoomReservations roomReserv)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != roomReserv.Id)
            {
                return BadRequest("Ids are not matching!");
            }

            db.Entry(roomReserv).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (db.AppRoomReservations.Find(id) == null)
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

        [HttpGet]
        [Route("ReservationPass/{userName}/{accommodationId}")]
        public bool ResservationPass(string userName, int accommodationId)
        {
            foreach (var reservation in db.AppRoomReservations)
            {
                var user = db.AppUsers.FirstOrDefault(p => p.UserName.Equals(userName));

                if (user == null)
                {
                    // korisnik nikada nije imao neku rezervaciju
                    return false;
                }

                if (reservation.AppUserId == user.Id)
                {
                    var room = db.AppRooms.Find(reservation.RoomId);

                    if (room != null)
                    {
                        if (accommodationId == room.AccommodationId)
                        {
                            // korisnik zeli da ostavi komentar na smestaj u kojem je boravio
                            //DateTime startDate = DateTime.ParseExact(reservation.StartDate.Split(' ')[0], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            string date = reservation.EndDate.Split('T')[0];
                            DateTime endDate = DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                            DateTime today = DateTime.Now;

                            if (endDate < today)
                            {
                                // korisnik je zavrsio boravak u smestaju i moze da ostavi komentar
                                return true;
                            }
                        }
                        else
                        {
                            // korisnik nije boravio u ovom smestaju
                            return false;
                        }
                    }
                }
            }
            return false;
        }

        [HttpPost]
        [Route("roomReservations")]
        [ResponseType(typeof(RoomReservations))]
        public IHttpActionResult PostPlace(RoomReservations roomReservations)
        {
            lock (obj)
            {
                using (var context = new BAContext())
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        if (!ModelState.IsValid)
                        {
                            return BadRequest(ModelState);
                        }

                        RoomReservations existingReservation = context.AppRoomReservations.Where(p => p.StartDate.Equals(roomReservations.StartDate) &&
                                                          p.EndDate.Equals(roomReservations.EndDate) &&
                                                          p.RoomId == roomReservations.RoomId).FirstOrDefault();

                        if (existingReservation != null)
                        {
                            return BadRequest("Reservation exists");
                        }

                        try
                        {
                            context.AppRoomReservations.Add(roomReservations);

                            context.SaveChanges();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            throw;
                        }

                        transaction.Commit();

                        return CreatedAtRoute("RoomReservationApi", new { id = roomReservations.Id }, roomReservations);
                    }
                }
            }
        }
    }
}