using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace KpopApi.Models
{
    public partial class Artist
    {
        public Artist()
        {
            Songs = new HashSet<Song>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        [Unicode(false)]
        public string Name { get; set; }
        [Required]
        [Column("Date_Debut")]
        public int DateDebut { get; set; }

        [InverseProperty("Artist")]
        public virtual ICollection<Song> Songs { get; set; }
    }
}
