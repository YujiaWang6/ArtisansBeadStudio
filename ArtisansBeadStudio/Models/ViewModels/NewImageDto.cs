using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArtisansBeadStudio.Models.ViewModels
{
    public class NewImageDto
    {
        public NewImageDto()
        {
            StyleList = new Dictionary<int, string>();
        }
        public Dictionary<int, string> StyleList { get; set; }
    }
}