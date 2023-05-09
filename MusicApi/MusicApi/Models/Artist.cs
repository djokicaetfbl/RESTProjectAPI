using System.ComponentModel.DataAnnotations.Schema;

namespace MusicApi.Models
{
    public class Artist
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public string ImagerUrl { get; set; }

        [NotMapped] //this property will not be part of source (SQL) table
        public IFormFile Image { get; set; }
        public ICollection<Album> Albums { get; set; }
    }
}
