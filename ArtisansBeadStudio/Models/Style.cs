using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArtisansBeadStudio.Models
{
    public class Style
    {
        public int StyleID { get; set; }
        public string StyleName { get; set; }

        //A Style can have many keychains related
        public ICollection<Keychain> Keychains { get; set; }
    }
    public class StyleDto
    {
        public int StyleID { get; set; }
        public string StyleName { get; set; }
    }
}