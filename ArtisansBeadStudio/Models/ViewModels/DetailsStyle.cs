using ArtisansBeadStudio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArtisansBeadStudio.Models.ViewModels
{
    public class DetailsStyle
    {
        public StyleDto SelectedStyle { get; set; }

        //include the keychains
        public IEnumerable<KeychainDto> keychainsInStyle { get; set; }
    }
}
