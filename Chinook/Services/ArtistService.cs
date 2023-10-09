using Chinook.Data;
using Chinook.Models;
using Microsoft.EntityFrameworkCore;

namespace Chinook.Services;

public class ArtistService
{
    private readonly IDbContextFactory<ChinookContext> _dbContextFactory;

    public ArtistService(IDbContextFactory<ChinookContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task<Artist?> GetArtistById(long id)
    =>
        (await GetAllArtists()).SingleOrDefault(a => a.ArtistId == id);

    public async Task<List<Artist>> GetArtists()
    => 
        (await GetAllArtists()).ToList();

    public async Task<List<Artist>> GetArtistsByName(string name)
    =>
        (await GetAllArtists())
        .Where(a => (a.Name ?? string.Empty).ToLower().Contains(name))
        .ToList();

    public async Task<List<Album>> GetAlbumsForArtist(int artistId)
    {
        using (var dbContext = await _dbContextFactory.CreateDbContextAsync())
        {
            return dbContext.Albums.Where(a => a.ArtistId == artistId).ToList();
        }
    }

    private async Task<IEnumerable<Artist>> GetAllArtists()
    {
        using (var dbContext = await _dbContextFactory.CreateDbContextAsync())
        {
            return dbContext
                .Artists
                .Include(a => a.Albums)
                .ToList();
        }
    }
}
