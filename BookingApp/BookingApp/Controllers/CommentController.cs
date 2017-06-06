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
    [RoutePrefix("api/comment")]
    public class CommentController : ApiController
    {
        private BAContext db = new BAContext();

        [HttpGet]
        [Route("comments", Name = "CommentApi")]
        public IHttpActionResult GetComment()
        {
            DbSet<Comment> comments = db.AppComments;

            if(comments == null)
            {
                return NotFound();
            }

            return Ok(comments);
        }

        [HttpPut]
        [Route("comments/{id}")]
        public IHttpActionResult PutComment(int id, Comment comment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != comment.Id)
            {
                return BadRequest("Ids are not matching!");
            }

            db.Entry(comment).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (db.AppComments.Find(id) == null)
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
        [Route("comments")]
        [ResponseType(typeof(Comment))]
        public IHttpActionResult PostComment(Comment comment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool commentExists = false;
            foreach (var item in db.AppComments)
            {
                if(item.AppUserId.Equals(comment.AppUserId)
                    && item.AccommodationId.Equals(comment.AccommodationId))
                {
                    commentExists = true;
                    break;
                }
            }

            if (commentExists == false)
            {
                db.AppComments.Add(comment);
                db.SaveChanges();

                return CreatedAtRoute("CountryApi", new { id = comment.Id }, comment);
            }
            else
            {
                return BadRequest();
            }

        }
    }
}
