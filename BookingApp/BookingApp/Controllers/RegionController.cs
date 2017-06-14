using BookingApp.Models;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;

namespace BookingApp.Controllers
{
    [RoutePrefix("api/region")]
    public class RegionController : ApiController
    {
        private BAContext db = new BAContext();

        [HttpGet]
        [Route("regions", Name = "RegionApi")]
        public IHttpActionResult GetRegions()
        {
            DbSet<Region> regions = db.AppRegions;

            if (regions == null)
            {
                return NotFound();
            }

            return Ok(regions);
        }

        [HttpPut]
        [Route("regions/{id}")]
        public IHttpActionResult PutRegion(int id, Region region)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != region.Id)
            {
                return BadRequest("Ids are not matching!");
            }

            db.Entry(region).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (db.AppRegions.Find(id) == null)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(region);
        }

        [HttpDelete]
        [Route("regions/{id}")]
        public IHttpActionResult DeleteRegion(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Region region  = new Region();

            foreach (var item in db.AppRegions)
            {
                if (id == item.Id)
                {
                    region = item;
                    break;
                }
            }

            if (region == null)
            {
                return BadRequest(ModelState);
            }

            db.AppRegions.Remove(region);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (db.AppRegions.Find(id) == null)
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
        [Route("regions")]
        [ResponseType(typeof(Region))]
        public IHttpActionResult PostRegion(Region region)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool regionExists = false;
            foreach (var item in db.AppRegions)
            {
                if(item.Name.Equals(region.Name) && item.CountryId.Equals(region.CountryId))
                {
                    regionExists = true;
                    break;
                }
            }

            if(regionExists == false)
            {
                db.AppRegions.Add(region);
                db.SaveChanges();

                return CreatedAtRoute("RegionApi", new { id = region.Id }, region);
            }
            else
            {
                return BadRequest();
            }

            
        }
    }
}