using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static ArtisansBeadStudio.Models.Image;

namespace ArtisansBeadStudio.Models.ViewModels
{
    public class EditImageDto : NewImageDto
    {
        public ImageDto SelectedImage { get; set; }
    }
}