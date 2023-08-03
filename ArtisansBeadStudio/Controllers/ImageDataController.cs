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
using static ArtisansBeadStudio.Models.Image;

namespace ArtisansBeadStudio.Controllers
{
    public class ImageDataController : ApiController
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/ImageData/ListImages
        [HttpGet]
        [ResponseType(typeof(ImageDto))]
        public IHttpActionResult ListImages(int id)
        {
            List<Image> Images = db.Images.Where(image => image.AlbumID == id).ToList();
            List<ImageDto> ImageDtos = new List<ImageDto>();

            Images.ForEach(image => ImageDtos.Add(new ImageDto()
            {
                ImageID = image.ImageID,
                ImageTitle = image.ImageTitle,
                ImageDescription = image.ImageDescription,
                AlbumName = image.Album.AlbumName,
                StyleName = image.Style.StyleName,
                AlbumId = image.Album.AlbumID,

            }));


            return Ok(ImageDtos);
        }

        // GET: api/ImageData/FindImage/2
        [ResponseType(typeof(ImageDto))]
        [HttpGet]
        public IHttpActionResult FindImage(int id)
        {
            Image Image = db.Images.Find(id);
            ImageDto ImageDto = new ImageDto()
            {
                ImageID = Image.ImageID,
                ImageTitle = Image.ImageTitle,
                ImageDescription = Image.ImageDescription,
                AlbumName = Image.Album.AlbumName,
                StyleName = Image.Style.StyleName,
                AlbumId = Image.Album.AlbumID,
                StyleID = Image.Style.StyleID,
                ImageData = Image.ImageData != null ?
                                "data:image / png; base64," + Convert.ToBase64String(Image.ImageData, 0, Image.ImageData.Length)
                                : null,
            };
            if (Image == null)
            {
                return NotFound();
            }

            return Ok(ImageDto);
        }

        // POST: api/ImageData/UpdateImage/2
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateImage(int id, Image image)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != image.ImageID)
            {
                return BadRequest();
            }

            db.Entry(image).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ImageExists(id))
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

        // POST: api/ImageData/AddImage
        [ResponseType(typeof(Image))]
        [HttpPost]
        public IHttpActionResult AddImage(Image image)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Images.Add(image);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = image.ImageID }, image);
        }

        // DELETE: api/ImageData/DeleteImage/5
        [ResponseType(typeof(Image))]
        [HttpPost]
        public IHttpActionResult DeleteImage(int id)
        {
            Image image = db.Images.Find(id);
            if (image == null)
            {
                return NotFound();
            }

            db.Images.Remove(image);
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

        private bool ImageExists(int id)
        {
            return db.Images.Count(e => e.ImageID == id) > 0;
        }
    }
}