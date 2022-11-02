using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace KpopApi.Models
{
    public partial class Song
    {
        [Key]
        public int Id { get; set; }
        [StringLength(100)]
        [Unicode(false)]
        public string Title { get; set; } = null!;
        public int ArtistId { get; set; }
        [Column(TypeName = "date")]
        public DateTime Release { get; set; }
        [Column(TypeName = "ntext")]
        public string? Summary { get; set; }

        [ForeignKey("ArtistId")]
        [InverseProperty("Songs")]
        public virtual Artist Artist { get; set; } = null!;
    }
}
