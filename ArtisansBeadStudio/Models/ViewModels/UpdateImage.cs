using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static ArtisansBeadStudio.Models.Image;

namespace ArtisansBeadStudio.Models.ViewModels
{
    public class UpdateImage
    {
        public ImageDto SelectedImage { get; set; }

        public IEnumerable<StyleDto> StyleOptions { get; set; }

        public IEnumerable<AlbumDto> AlbumOptions { get; set; }
    }
}