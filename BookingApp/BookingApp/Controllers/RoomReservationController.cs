using BookingApp.Models;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace BookingApp.Controllers
{
    [RoutePrefix("api/RoomReservation")]
    public class RoomReservationController : ApiController
    {
        private BAContext db = new BAContext();
        public static object obj = new object();

        //User manager -> We will use it to check role if needed.
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

        [AllowAnonymous]
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

        [Authorize]
        [HttpPost]
        [Route("roomReservations")]
        [ResponseType(typeof(RoomReservations))]
        public IHttpActionResult PostReservation(RoomReservations roomReservations)
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

                        //RoomReservations existingReservation = context.AppRoomReservations.Where(p => p.StartDate.Equals(roomReservations.StartDate) &&
                        //                                  p.EndDate.Equals(roomReservations.EndDate) &&
                        //                                  p.RoomId == roomReservations.RoomId).FirstOrDefault();

                        bool existingReservation = false;

                        foreach (var reservation in context.AppRoomReservations)
                        {
                            if (reservation.RoomId == roomReservations.RoomId)
                            {
                                string date = reservation.EndDate.Split('T')[0];
                                DateTime endDate = DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                                date = reservation.StartDate.Split('T')[0];
                                DateTime startDate = DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                                DateTime currentEndDate = DateTime.ParseExact(roomReservations.EndDate.Split('T')[0],
                                                                               "yyyy-MM-dd", CultureInfo.InvariantCulture);

                                DateTime currentStartDate = DateTime.ParseExact(roomReservations.StartDate.Split('T')[0],
                                                                              "yyyy-MM-dd", CultureInfo.InvariantCulture);

                                if (currentStartDate >= startDate && currentStartDate <= endDate)
                                {
                                    existingReservation = true;
                                }
                            }
                        }

                        if (existingReservation)
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

        [Authorize]
        [HttpPut]
        [Route("roomReservations")]
        [ResponseType(typeof(RoomReservations))]
        public IHttpActionResult UpdateReservation(RoomReservations roomReservations)
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

                        //bool isAdmin = UserManager.IsInRole(User.Identity.Name, "Admin");//User.Identity.Name => Username Identity User-a! UserManager trazi po njegovom username-u, i onda poredi!
                        var user = context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);//Vadimo iz Identity baze po username-u Identity User-a, koji u sebi sadrzi AppUser-a!
                        if (user != null && user.appUserId.Equals(roomReservations.AppUserId))//Ako korisnik nije admin, i nije AppUser koji trazi podatke o sebi, nije autorizovan!
                        {
                            // korisnik ima pravo izmene svoje rezervacije
                            bool existingReservation = false;

                            foreach (var reservation in context.AppRoomReservations)
                            {
                                if (reservation.RoomId == roomReservations.RoomId)
                                {
                                    string date = reservation.EndDate.Split('T')[0];
                                    DateTime endDate = DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                                    date = reservation.StartDate.Split('T')[0];
                                    DateTime startDate = DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                                    DateTime currentEndDate = DateTime.ParseExact(roomReservations.EndDate.Split('T')[0],
                                                                                   "yyyy-MM-dd", CultureInfo.InvariantCulture);

                                    DateTime currentStartDate = DateTime.ParseExact(roomReservations.StartDate.Split('T')[0],
                                                                                  "yyyy-MM-dd", CultureInfo.InvariantCulture);

                                    if (currentStartDate >= startDate && currentStartDate <= endDate)
                                    {
                                        existingReservation = true;
                                    }
                                }
                            }

                            if (existingReservation)
                            {
                                return BadRequest("Reservation exists");
                            }

                            try
                            {
                                //context.AppRoomReservations.Add(roomReservations);
                                var reservation = context.AppRoomReservations.Find(roomReservations.Id);

                                reservation.EndDate = roomReservations.EndDate;
                                reservation.StartDate = roomReservations.StartDate;
                                reservation.RoomId = roomReservations.RoomId;

                                context.SaveChanges();
                            }
                            catch (DbUpdateConcurrencyException)
                            {
                                throw;
                            }
                        }

                        transaction.Commit();

                        return CreatedAtRoute("RoomReservationApi", new { id = roomReservations.Id }, roomReservations);
                    }
                }
            }
        }

        [Authorize]
        [HttpDelete]
        [Route("roomReservations/delete/{id}")]
        public IHttpActionResult DeleteReservation(int id)
        {
            var reservation = db.AppRoomReservations.Find(id);

            if (reservation == null)
            {
                return BadRequest();
            }

            db.AppRoomReservations.Remove(reservation);

            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}