using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicApi.Data;
using MusicApi.Models;

namespace MusicApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlbumsController : ControllerBase
    {
        private ApiDbContext _dbContext;

        public AlbumsController(ApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] Album album)
        {
            //var imageUrl = await FileHelper.UploadImage(album.Image);
            //album.ImageUrl = imageUrl;
            //var audioUrl = await FileHelper.UploadFile(album.AudioFile);
            //album.AudioUrl = audioUrl;
            //album.UploadedDate = DateTime.Now;
            await _dbContext.Albums.AddAsync(album);
            await _dbContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpGet]
        public async Task<IActionResult> GetAlbums(int? pageNumber, int? pageSize)
        {
            int currentPageNumber = pageNumber ?? 1;
            int currentPageSize = pageSize ?? 2;
            var albums = await _dbContext.Albums.Select(x => new
            {
                Id = x.Id,
                Name = x.Name,
                ImageUrl = x.ImageUrl
            }).ToListAsync();
            //var albums = _dbContext.Albums;
            return Ok(albums.Skip((currentPageNumber - 1) * currentPageSize).Take(currentPageSize));  
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> AlbumDetails(int albumId)
        {
            var albumsDetails = await _dbContext.Albums
                .Where(x => x.Id == albumId)
                .Include(x => x.Songs).ToListAsync();
            return Ok(albumsDetails);
        }
    }
}
