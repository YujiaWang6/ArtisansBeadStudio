using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ArtisansBeadStudio.Models
{
    public class BeadColour
    {
        [Key]
        public int ColourId { get; set; }
        public string ColourName { get; set; }
        public string ColourProperty { get; set; }

    }
    public class BeadColourDto
    {
        public int ColourId { get; set; }
        public string ColourName { get; set; }
        public string ColourProperty { get; set; }
    }
}