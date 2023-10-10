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
        {
                return _dbContext
                    .Playlists
                    .ToList();
        }

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

            playlistModel.Tracks.Add(track);
            _dbContext.Update(playlistModel);

            await _dbContext.SaveChangesAsync();
        }

        public async Task CreatePlaylist(Models.Playlist playlist)
        {
            _dbContext.Playlists.Add(playlist);
            await _dbContext.SaveChangesAsync();
        }

        public async Task CreateUserPlaylist(Models.Playlist? playlistModel, string userId)
        {
            if (!_dbContext.UserPlaylists.Any(p => p.PlaylistId == playlistModel.PlaylistId && p.UserId == userId))
            { 
                UserPlaylist userPlaylist = new()
                {
                    User = _dbContext.Users.FirstOrDefault(u => u.Id == userId),
                    Playlist = playlistModel
                };

                _dbContext.UserPlaylists.Add(userPlaylist);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<Models.Playlist?> GetPlaylistByName(string name)
        {
            return _dbContext
                .Playlists
                .Include (p => p.Tracks)
                .FirstOrDefault(u => u.Name == name);
        }
    }
}
