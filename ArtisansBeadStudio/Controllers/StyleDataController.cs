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


        /// <summary>
        /// Return All the styles contain a specific keychain
        /// </summary>
        /// <param name="id">Keychain primary key</param>
        /// <returns>
        /// HEADER: 200(OK)
        /// CONTENT: all styles in the database that contain a particular keychain
        /// </returns>
        /// <example>
        /// GET: api/StyleData/ListStylesForKeychain/1
        /// </example>
        /// (collaboration part) 
        [HttpGet]
        [ResponseType(typeof(StyleDto))]
        public IHttpActionResult ListStylesForKeychain(int id)
        {
            List<Style> Styles = db.Styles.Where(
                s=>s.Keychains.Any(
                    k=>k.KeychainId == id)
                ).ToList();
            List<StyleDto> StyleDtos = new List<StyleDto>();

            Styles.ForEach(s => StyleDtos.Add(new StyleDto()
            {
                StyleID = s.StyleID,
                StyleName = s.StyleName
            }));

            return Ok(StyleDtos);
        }



        /// <summary>
        /// Return All the styles NOT contain a specific keychain
        /// </summary>
        /// <param name="id">Keychain primary key</param>
        /// <returns>
        /// HEADER: 200(OK)
        /// CONTENT: all styles in the database that NOT contain a particular keychain
        /// </returns>
        /// <example>
        /// GET: api/StyleData/ListStylesNotIncludeKeychain/1
        /// </example>
        /// (collaboration part) 
        [HttpGet]
        [ResponseType(typeof(StyleDto))]
        public IHttpActionResult ListStylesNotIncludeKeychain(int id)
        {
            List<Style> Styles = db.Styles.Where(
                s => !s.Keychains.Any(
                    k => k.KeychainId == id)
                ).ToList();
            List<StyleDto> StyleDtos = new List<StyleDto>();

            Styles.ForEach(s => StyleDtos.Add(new StyleDto()
            {
                StyleID = s.StyleID,
                StyleName = s.StyleName
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
        // POST: api/StyleData/UpdateStyle/2
        [ResponseType(typeof(void))]
        [HttpPost]
        [Authorize]
        public IHttpActionResult UpdateStyle(int id, Style style)
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

        // POST: api/StyleData/AddStyle
        [ResponseType(typeof(Style))]
        [HttpPost]
        //[Authorize]
        public IHttpActionResult AddStyle(Style style)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Styles.Add(style);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = style.StyleID }, style);
        }

        // DELETE: api/StyleData/DeleteStyle/5
        [ResponseType(typeof(Style))]
        [HttpPost]
        public IHttpActionResult DeleteStyle(int id)
        {
            Style style = db.Styles.Find(id);
            if (style == null)
            {
                return NotFound();
            }

            db.Styles.Remove(style);
            db.SaveChanges();

            return Ok();
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