using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using KpopApi.Data;
using KpopApi.Models;
using Path = System.IO.Path;

namespace KpopApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SeedController : ControllerBase
{
    private readonly KpopContext _context;
    private readonly string _pathName;
    private readonly IConfiguration _configuration;

    public SeedController(KpopContext context, IHostEnvironment environment, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
        _pathName = Path.Combine(environment.ContentRootPath, "Data/kpop.csv");
    }

    [HttpGet("Songs")]
    public async Task<IActionResult> ImportSongs()
    {
        Dictionary<string, Artist> Artists = await _context.Artists.AsNoTracking()
            .ToDictionaryAsync(c => c.Name);

        CsvConfiguration config = new(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            HeaderValidated = null
        };
        int songCount = 0;
        using (StreamReader reader = new(_pathName))
        using (CsvReader csv = new(reader, config))
        {
            IEnumerable<KpopCsv>? records = csv.GetRecords<KpopCsv>();
            foreach (KpopCsv record in records)
            {
                if (!Artists.ContainsKey(record.artist))
                {
                    Console.WriteLine($"Not found artist for {record.song}");
                    return NotFound(record);
                }


                Song song = new()
                {
                    Title = record.song,
                    Release = record.release,
                    //Summary = record.summary,
                    ArtistId = Artists[record.artist].Id
                };
                _context.Songs.Add(song);
                songCount++;
            }
            await _context.SaveChangesAsync();
        }

        return new JsonResult(songCount);
    }

    [HttpGet("Artists")]
    public async Task<IActionResult> ImportArtists()
    {
        // create a lookup dictionary containing all the countries already existing 
        // into the Database (it will be empty on first run).
        Dictionary<string, Artist> artistsByName = _context.Artists
            .AsNoTracking().ToDictionary(x => x.Name, StringComparer.OrdinalIgnoreCase);

        CsvConfiguration config = new(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            HeaderValidated = null
        };
        using (StreamReader reader = new(_pathName))
        using (CsvReader csv = new(reader, config))
        {
            IEnumerable<KpopCsv>? records = csv.GetRecords<KpopCsv>();
            foreach (KpopCsv record in records)
            {
                if (artistsByName.ContainsKey(record.artist))
                {
                    continue;
                }

                Artist artist = new()
                {
                    Name = record.artist,
                    DateDebut = record.debut
                };
                await _context.Artists.AddAsync(artist);
                artistsByName.Add(record.artist, artist);
            }

            await _context.SaveChangesAsync();
        }
        return new JsonResult(artistsByName.Count);
    }

}


