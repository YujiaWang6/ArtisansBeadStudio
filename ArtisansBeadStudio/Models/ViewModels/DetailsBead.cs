﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArtisansBeadStudio.Models.ViewModels
{
    public class DetailsBead
    {
        public BeadDto specificBead { get; set; }

        public IEnumerable<KeychainDto> associatedKeychain { get; set; }


        public IEnumerable<KeychainDto> aviliableKeychain { get; set; }
    }
}