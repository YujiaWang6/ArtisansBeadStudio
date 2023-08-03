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
using ArtisansBeadStudio.Migrations;

namespace ArtisansBeadStudio.Controllers
{
    public class AlbumDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/AlbumData/ListAlbums
        [HttpGet]
        [ResponseType(typeof(AlbumDto))]
        public IHttpActionResult ListAlbums()
        {
            List<Album> Albums = db.Albums.ToList();
            List<AlbumDto> AlbumDtos = new List<AlbumDto>();
            Albums.ForEach(album => AlbumDtos.Add(new AlbumDto()
            {
                AlbumID = album.AlbumID,
                AlbumName = album.AlbumName,
            }));

            return Ok(AlbumDtos);
        }

        // PUT: api/AlbumData/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAlbum(int id, Album album)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != album.AlbumID)
            {
                return BadRequest();
            }

            db.Entry(album).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AlbumExists(id))
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

        // POST: api/AlbumData
        [ResponseType(typeof(Album))]
        public IHttpActionResult PostAlbum(Album album)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Albums.Add(album);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = album.AlbumID }, album);
        }

        // DELETE: api/AlbumData/5
        [ResponseType(typeof(Album))]
        public IHttpActionResult DeleteAlbum(int id)
        {
            Album album = db.Albums.Find(id);
            if (album == null)
            {
                return NotFound();
            }

            db.Albums.Remove(album);
            db.SaveChanges();

            return Ok(album);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AlbumExists(int id)
        {
            return db.Albums.Count(e => e.AlbumID == id) > 0;
        }
    }
}