﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArtisansBeadStudio.Models.ViewModels
{
    public class DetailsKeychain
    {
        public KeychainDto specificKeychain { get; set; }

        public IEnumerable<BeadDto> beadsInKeychain { get; set; }
    }
}