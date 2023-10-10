using Chinook.Data;
using Microsoft.EntityFrameworkCore;

namespace Chinook.Services;

public class ArtistService
{
    private readonly IDbContextFactory<ChinookContext> _dbContextFactory;
    private readonly ChinookContext _dbContext;

    public ArtistService(IDbContextFactory<ChinookContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
        _dbContext = _dbContextFactory.CreateDbContext();
    }

    public async Task<ClientModels.Artist?> GetArtistById(long id)
    =>
        (await GetAllArtists()).SingleOrDefault(a => a.ArtistId == id);

    public async Task<List<ClientModels.Artist>> GetArtists()
    =>
        (await GetAllArtists())
        .ToList();

    public async Task<List<ClientModels.Artist>> GetArtistsByName(string name)
    =>
        (await GetAllArtists())
        .Where(a => (a.Name ?? string.Empty).ToLower().Contains(name))
        .ToList();

    private async Task<IEnumerable<ClientModels.Artist>> GetAllArtists()
    =>
        await _dbContext
        .Artists
        .Include(a => a.Albums)
        .Select(a => new ClientModels.Artist()
        {
            ArtistId = a.ArtistId,
            Name = a.Name ?? string.Empty,
            AlbumCount = a.Albums.Count
        })
        .ToListAsync();
}
