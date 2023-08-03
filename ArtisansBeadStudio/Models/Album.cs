using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace ArtisansBeadStudio.Models
{
    public class Album
    {
        [Key]
        public int AlbumID { get; set; }
        public string AlbumName { get; set; }
    }
    public class AlbumDto
    {
        public int AlbumID { get; set; }
        public string AlbumName { get; set; }
    }
}