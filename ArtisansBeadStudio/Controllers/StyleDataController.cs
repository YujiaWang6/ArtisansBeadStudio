using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using ArtisansBeadStudio.Models;

namespace ArtisansBeadStudio.Controllers
{
    public class StyleDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/StyleData/ListStyles
        [HttpGet]
        [ResponseType(typeof(StyleDto))]
        public IHttpActionResult ListStyles()
        {
            List<Style> Styles = db.Styles.ToList();
            List<StyleDto> StyleDtos = new List<StyleDto>();

            Styles.ForEach(style => StyleDtos.Add(new StyleDto()
            {
                StyleID = style.StyleID,
                StyleName = style.StyleName,
            }));

            return Ok(StyleDtos);
        }

        // GET: api/StyleData/FindStyle/2
        [ResponseType(typeof(StyleDto))]
        [HttpGet]
        public IHttpActionResult FindStyle(int id)
        {
            Style Style = db.Styles.Find(id);
            StyleDto StyleDto = new StyleDto()
            {
                StyleID = Style.StyleID,
                StyleName = Style.StyleName,
            };
            if (Style == null)
            {
                return NotFound();
            }

            return Ok(StyleDto);
        }
        // PUT: api/StyleData/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutStyle(int id, Style style)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != style.StyleID)
            {
                return BadRequest();
            }

            db.Entry(style).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StyleExists(id))
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

        // POST: api/StyleData
        [ResponseType(typeof(Style))]
        public IHttpActionResult PostStyle(Style style)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Styles.Add(style);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = style.StyleID }, style);
        }

        // DELETE: api/StyleData/5
        [ResponseType(typeof(Style))]
        public IHttpActionResult DeleteStyle(int id)
        {
            Style style = db.Styles.Find(id);
            if (style == null)
            {
                return NotFound();
            }

            db.Styles.Remove(style);
            db.SaveChanges();

            return Ok(style);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StyleExists(int id)
        {
            return db.Styles.Count(e => e.StyleID == id) > 0;
        }
    }
}