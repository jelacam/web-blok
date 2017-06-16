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

        [AllowAnonymous]
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

        [Authorize]
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

        [Authorize]
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
                

                var accommodation = db.AppAccommodations.Find(comment.AccommodationId);

                if (accommodation != null)
                {
                    db.AppAccommodations.Attach(accommodation);
                    int commentCount = 1;
                    if (accommodation.Comment != null)
                    {
                        commentCount += accommodation.Comment.Count;
                    }
                    

                    accommodation.AverageGrade = (accommodation.AverageGrade + comment.Grade) / commentCount;
                    
                }
                else
                {
                    return NotFound();
                }

                db.AppComments.Add(comment);
                db.Entry(accommodation).State = EntityState.Modified;
                db.SaveChanges();

                return CreatedAtRoute("CountryApi", new { id = comment.Id }, comment);
            }
            else
            {
                return BadRequest();
            }

        }

        [AllowAnonymous]
        [HttpGet]
        [Route("comments/{accomId}")]
        public IHttpActionResult GetAccomComments(int accomId)
        {

            DbSet<Comment> comments = db.AppComments;
            List<Comment> accomComments = new List<Comment>(10);

            if (comments == null)
            {
                return NotFound();
            }

            foreach(var comment in comments)
            {
                if (comment.AccommodationId == accomId)
                {
                    accomComments.Add(comment);
                }
            }

            if (accomComments.Count > 0)
            {
                return Ok(accomComments);
            }
            else
            {
                return NotFound();
            }

            

        }
    }
}
