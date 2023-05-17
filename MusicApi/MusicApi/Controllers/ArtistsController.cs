using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicApi.Data;
using MusicApi.Models;

namespace MusicApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistsController : ControllerBase
    {
        private ApiDbContext _dbContext;

        public ArtistsController(ApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //POST api/<ArtistsController>
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] Artist artist)
        {
            //var imageUrl = await FileHelper.UploadImage(artist.Image);
            //artist.ImageUrl = imageUrl;
            //var audioUrl = await FileHelper.UploadFile(artist.AudioFile);
            //artist.AudioUrl = audioUrl;
            //artist.UploadedDate = DateTime.Now;
            await _dbContext.Artists.AddAsync(artist);
            await _dbContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created);
        }

        //api/artists

        [HttpGet]
        public async Task<IActionResult> GetArtists(int? pageNumber, int? pageSize)
        {
            //var artists = _dbContext.Artists;
            //return Ok(artists);
            int currentPageNumber = pageNumber ?? 1;
            int currentPageSize = pageSize ?? 2;
            var artists = await (from artist in _dbContext.Artists
                                 select new
                                 {
                                     Id = artist.Id,
                                     ArtistName = artist.Name,
                                     Image = artist.ImagerUrl
                                 }
            ).ToListAsync();
            return Ok(artists.Skip((currentPageNumber - 1) * currentPageSize).Take(currentPageSize));
        }

        [HttpGet("[action]")] // /api/artists/artistsdetails
        public async Task<IActionResult> ArtistDetails(int artiststId)  // /api/artists/artistsdetails?artiststId=1
        {
            var artistDetails = await _dbContext.Artists
                .Where(x => x.Id == artiststId)
                .Include(a => a.Songs).ToListAsync();
            return Ok(artistDetails);
        }
    }
}
