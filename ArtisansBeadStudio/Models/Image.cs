using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ArtisansBeadStudio.Models
{
    public class Image
    {
        [Key]
        public int ImageID { get; set; }
        public string ImageTitle { get; set; }
        public string ImageDescription { get; set; }
        public byte[] ImageData { get; set; }

        [ForeignKey("Style")]
        public int StyleID { get; set; }
        public virtual Style Style { get; set; }

        [ForeignKey("Album")]
        public int AlbumID { get; set; }
        public virtual Album Album { get; set; }
    }
    public class ImageDto
    {
            public int ImageID { get; set; }
            public string ImageTitle { get; set; }
            public string ImageDescription { get; set; }
            public string ImageData { get; set; }
            public string AlbumName { get; set; }

            public int AlbumId { get; set; }
            public int StyleID { get; set; }
            public string StyleName { get; set; }
    }

}