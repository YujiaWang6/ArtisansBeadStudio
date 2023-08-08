using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArtisansBeadStudio.Models.ViewModels
{
    public class DetailsKeychain
    {
        public KeychainDto specificKeychain { get; set; }

        public IEnumerable<BeadDto> beadsInKeychain { get; set; }

        //add all the associated styles into viewmodel (collaboration)
        public IEnumerable<StyleDto> associatedStyles { get; set;}

        //add all aviliable styles into viewmodel (collaboration)
        public IEnumerable<StyleDto> aviliableStyles { get; set; }
    }
}