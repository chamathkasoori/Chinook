using Chinook.Models;
using Chinook.Data;
using Microsoft.EntityFrameworkCore;

namespace Chinook.Services
{
    public class PlaylistService
    {
        private readonly IDbContextFactory<ChinookContext> _dbContextFactory;
        private readonly ChinookContext _dbContext;

        public PlaylistService(IDbContextFactory<ChinookContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
            _dbContext = _dbContextFactory.CreateDbContext();
        }

        public async Task<List<Models.Playlist>> GetAllPlaylists()
        =>
            await _dbContext
                    .Playlists
                    .ToListAsync();

        public async Task<Models.Playlist?> GetPlaylistByName(string name)
        =>
            await _dbContext
                    .Playlists
                    .Include(p => p.Tracks)
                    .FirstOrDefaultAsync(u => u.Name == name);

        public async Task<List<ClientModels.Playlist>> GetUserPlaylists(string userId)
        =>
            await _dbContext
                    .UserPlaylists
                    .Include(up => up.Playlist)
                    .Where(up => up.UserId == userId)
                    .Select(up => new ClientModels.Playlist()
                    {
                        Id = up.Playlist.PlaylistId,
                        Name = up.Playlist.Name?? string.Empty,
                    })
                    .ToListAsync();

        public async Task<ClientModels.Playlist?> GetPlaylistById(long playlistId, string userId)
        =>
            await _dbContext.Playlists
                .Include(a => a.Tracks).ThenInclude(a => a.Album).ThenInclude(a => a.Artist)
                .Where(p => p.PlaylistId == playlistId)
                .Select(p => new ClientModels.Playlist()
                {
                    Name = p.Name ?? string.Empty,
                    Tracks = p.Tracks.Select(t => new ClientModels.PlaylistTrack()
                    {
                        AlbumTitle = t.Album!.Title,
                        ArtistName = t.Album.Artist.Name ?? string.Empty,
                        TrackId = t.TrackId,
                        TrackName = t.Name,
                        IsFavorite = t.Playlists.Where(p => p.UserPlaylists.Any(up => up.UserId == userId && up.Playlist.Name == "Favorites")).Any()
                    }).ToList()
                })
                .FirstOrDefaultAsync();


        public async Task AddTrackToPlayList(ClientModels.Playlist playlist, string userId)
        {
            Models.Playlist? playlistModel = null;

            if (!string.IsNullOrEmpty(playlist.Name)
                && !_dbContext.Playlists.Any(p => p.Name == playlist.Name)
            )
            {
                playlistModel = new()
                {
                    PlaylistId = (_dbContext.Playlists.Max(p => p.PlaylistId)) + 1,
                    Name = playlist.Name
                };
                await CreatePlaylist(playlistModel);
            }
            else
            {
                playlistModel = _dbContext
                    .Playlists
                    .Include(p => p.Tracks)
                    .FirstOrDefault(p => p.PlaylistId == playlist.Id);
            }

            await CreateUserPlaylist(playlistModel, userId);

            var track = _dbContext.Tracks.FirstOrDefault(t => t.TrackId == playlist.Tracks.First().TrackId);

            if (playlistModel != null && track != null) 
            { 
                playlistModel.Tracks.Add(track);
                _dbContext.Update(playlistModel);

                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task CreatePlaylist(Models.Playlist playlist)
        {
            _dbContext.Playlists.Add(playlist);
            await _dbContext.SaveChangesAsync();
        }

        public async Task CreateUserPlaylist(Models.Playlist? playlistModel, string userId)
        {
            if (!_dbContext.UserPlaylists.Any(p => p.PlaylistId == playlistModel!.PlaylistId && p.UserId == userId))
            {
                UserPlaylist userPlaylist = new()
                {
                    User = _dbContext.Users.FirstOrDefault(u => u.Id == userId)!,
                    Playlist = playlistModel!
                };

                _dbContext.UserPlaylists.Add(userPlaylist);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task RemoveTrackFromPlaylist(long playlistId, long trackId)
        {
            var playlist = _dbContext.
                Playlists.
                Include(a => a.Tracks).
                FirstOrDefault(p => p.PlaylistId == playlistId);

            var track = _dbContext
                .Tracks
                .FirstOrDefault(t => t.TrackId == trackId);

            if (playlist != null && track != null)
            {
                playlist.Tracks.Remove(track);

                _dbContext.Update(playlist);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
