﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicApi.Data;
using MusicApi.Helpers;
using MusicApi.Models;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MusicApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SongsController : ControllerBase
    {
        private readonly ApiDbContext _dbContext;
        public SongsController(ApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] Song song) // because we want upload file also 
        {
            //var imageUrl = await FileHelper.UploadImage(song.Image);
            //song.ImageUrl = imageUrl;
            //var audioUrl = await FileHelper.UploadFile(song.AudioFile);
            //song.AudioUrl = audioUrl;
            //song.UploadedDate = DateTime.Now;
            song.UploadedDate = DateTime.Now;
            await _dbContext.Songs.AddAsync(song);
            await _dbContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSongs(int? pageNumber, int? pageSize) // use paging
        {
           int currentPageNumber = pageNumber ?? 1;
           int currentPageSize = pageSize ?? 2;
           var songs = await _dbContext.Songs
                .Select(s => new
            {
                Id = s.Id,
                Title = s.Title,
                Duration = s.Duration,
                ImageUrl = s.ImageUrl,
                AudioUrl = s.AudioUrl
            }).ToListAsync();
            //var songs = _dbContext.Songs;
            return Ok(songs.Skip((currentPageNumber - 1) * currentPageSize).Take(currentPageSize));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> FeaturedSongs()
        {
            var songs = await _dbContext.Songs
                .Where(x => x.IsFeatured)
                .Select(s => new
            {
                Id = s.Id,
                Title = s.Title,
                Duration = s.Duration,
                ImageUrl = s.ImageUrl,
                AudioUrl = s.AudioUrl
            }).ToListAsync();
            //var songs = _dbContext.Songs;
            return Ok(songs);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> NewSongs()
        {
            var songs = await _dbContext.Songs
                .OrderByDescending(x => x.UploadedDate)
                .Select(s => new
                {
                    Id = s.Id,
                    Title = s.Title,
                    Duration = s.Duration,
                    ImageUrl = s.ImageUrl,
                    AudioUrl = s.AudioUrl
                }).Take(3).ToListAsync();
            //var songs = _dbContext.Songs;
            return Ok(songs);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> SearchSongs(string query)
        {
            var songs = await _dbContext.Songs
                .Where(x => x.Title.StartsWith(query))
                .Select(s => new
                {
                    Id = s.Id,
                    Title = s.Title,
                    Duration = s.Duration,
                    ImageUrl = s.ImageUrl,
                    AudioUrl = s.AudioUrl
                }).Take(15).ToListAsync();
            //var songs = _dbContext.Songs;
            return Ok(songs);
        }

        //// GET: api/<SongsController>
        //[HttpGet]
        //public async /*IEnumerable<Song>*/ Task<IActionResult> Get()
        //{
        //    return Ok(await _dbContext.Songs.ToListAsync());
        //    //return Ok(_dbContext.Songs);
        //    //return BadRequest();
        //    //return NotFound();
        //    //return StatusCode(200);
        //    //return StatusCode(StatusCodes.Status200OK);
        //}

        //// GET api/<SongsController>/5
        //[HttpGet("{id}")]
        //public async Task<IActionResult> /*Song*/ Get(int id)
        //{
        //    var song = await _dbContext.Songs.FindAsync(id);//.FirstOrDefault(x => x.Id == id);
        //    if(song == null)
        //    {
        //        return NotFound("No record found against this 'Id'.");
        //    }
        //    else
        //    {
        //        return Ok(song);
        //    }
        //}

        //// POST api/<SongsController>
        ////[HttpPost]
        ////public async Task<IActionResult> Post([FromBody] Song song)
        ////{
        ////    await _dbContext.Songs.AddAsync(song);
        ////    await _dbContext.SaveChangesAsync();
        ////    return StatusCode(StatusCodes.Status201Created);
        ////}

        //[HttpPost]
        //public async Task<IActionResult> Post([FromForm] Song song) // because we want upload file also 
        //{
        //    //var imageUrl = await FileHelper.UploadImage(song.Image);
        //    //song.ImageUrl = imageUrl;
        //    //var audioUrl = await FileHelper.UploadFile(song.AudioFile);
        //    //song.AudioUrl = audioUrl;
        //    //song.UploadedDate = DateTime.Now;
        //    await _dbContext.Songs.AddAsync(song);
        //    await _dbContext.SaveChangesAsync();
        //    return StatusCode(StatusCodes.Status201Created);
        //}

        //// PUT api/<SongsController>/5
        //[HttpPut("{id}")]
        //public async Task<IActionResult> Put(int id, [FromBody] Song song)
        //{
        //    var songObject = await _dbContext.Songs.FindAsync(id);
        //    if(songObject == null)
        //    {
        //        return NotFound("No record found against this 'Id'.");
        //    } 
        //    else
        //    {
        //        songObject.Title = song.Title;
        //        songObject.Language = song.Language;
        //        songObject.Duration = song.Duration;
        //        await _dbContext.SaveChangesAsync();
        //        return Ok("Record updated successfully.");
        //    }
        //}

        //// DELETE api/<SongsController>/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    var song = await _dbContext.Songs.FindAsync(id);
        //    if(song == null)
        //    {
        //        return NotFound("No record found against this 'Id'.");
        //    } 
        //    else
        //    {
        //        _dbContext.Remove(song);
        //        await _dbContext.SaveChangesAsync();
        //        return Ok("Record deleted successfully.");
        //    }

        //}

        ////api/songs/test/1 // this is how we approach the method Test(int id) // this is ROUTING
        //[HttpGet("[action]/{id}")]
        //public int Test(int id)
        //{
        //    return id;
        //}
    }
}
