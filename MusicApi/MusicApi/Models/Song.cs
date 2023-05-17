using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicApi.Models
{
    public class Song
    {
        //public int Id { get; set; }
        //[Required(ErrorMessage = "Title can not be null or empty")] // this is data validation
        //public string Title { get; set; }
        //[Required(ErrorMessage = "Language cannnot be null or empty")]
        //public string Language { get; set; }
        //[Required(ErrorMessage = "Kindly provide the song duration")]
        //public string Duration { get; set; }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Duration { get; set; }
        public DateTime UploadedDate { get; set; }
        public bool IsFeatured { get; set; }

        [NotMapped] //this property will not be part of source (SQL) table
        public IFormFile Image { get; set; }
        public string? ImageUrl { get; set; }
        [NotMapped]
        public IFormFile AudioFile { get; set; }
        public string? AudioUrl { get; set; }
        public int ArtistId { get; set; }
        public int? AlbumId { get; set; } // postoji pjesma koja nije ni na jednom albumu

    }
}
