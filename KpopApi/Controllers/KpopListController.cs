using KpopApi;
using KpopApi.Models;
using Microsoft.AspNetCore.Mvc;
namespace KpopApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class KpopListController : ControllerBase
    {
        private static readonly string[] ArtistsList = new[]
        {
        "BTS", "BLACK PINK", "PSY", "TWICE", "IVE", "BIGBANG"
    };
        private static readonly string[] SongsList = new[]
        {
        "Butter", "Pink Venom", "That That", "Talk that Talk", "Love Dive", "Still Life"
    };

        private readonly ILogger<KpopListController> _logger;

        public KpopListController(ILogger<KpopListController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Artist> GetArtists()
        {
            return Enumerable.Range(1, ArtistsList.Length).Select((v,i) => new Artist
            {
                Name = ArtistsList[i]
            })
            .ToArray();
        }

        [HttpGet]
        public IEnumerable<Song> GetSongs()
        {
            return Enumerable.Range(1, SongsList.Length).Select((v,i) => new Song
            {
                Title = SongsList[i]
            })
            .ToArray();
        }

    }
}

/*
namespace KpopApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class KpopListController : ControllerBase
    {
        private static readonly string[] Singers = new[]
        {
        "BTS", "BLACK PINK", "PSY", "TWICE", "IVE", "BIGBANG"
    };
        private static readonly string[] Songs = new[]
        {
        "Butter", "Pink Venom", "That That", "Talk that Talk", "Love Dive", "Still Life"
    };

        private readonly ILogger<KpopListController> _logger;

        public KpopListController(ILogger<KpopListController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<string> GetSingers()
        {
            return Singers;
        }

        [HttpGet]
        public IEnumerable<string> GetSongs()
        {
            return Songs;
        }
         
    }
}*/

/*
[HttpGet]
public IEnumerable<WeatherForecast> GetWeatherforecast()
{
    return Enumerable.Range(1, 5).Select(index => new WeatherForecast
    {
        Date = DateTime.Now.AddDays(index),
        TemperatureC = Random.Shared.Next(-20, 55),
        Summary = Summaries[Random.Shared.Next(Summaries.Length)]
    })
    .ToArray();
}

[HttpGet]
public IEnumerable<string> GetAttributes()
{
    return Summaries;
}
*/