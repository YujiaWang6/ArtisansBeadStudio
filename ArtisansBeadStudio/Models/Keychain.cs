using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArtisansBeadStudio.Models
{
    public class Keychain
    {

        [Key]
        public int KeychainId { get; set; }
        public string KeychainName { get; set; }

        //A keychain can have many beads
        public ICollection<Bead> Beads { get; set; }

        //A keychain can have many styles
        public ICollection<Style> Styles { get; set; }

        //Create another column for the user
        [ForeignKey("ApplicationUser")]
        public string UserID { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }

    public class KeychainDto
    {
        public int KeychainId { get; set; }
        public string KeychainName { get; set; }
        public string UserID { get; set; }
    }

}