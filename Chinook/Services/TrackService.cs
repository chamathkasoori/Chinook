using Chinook.Data;
using Chinook.Models;
using Microsoft.EntityFrameworkCore;
using PlaylistTrack = Chinook.ClientModels.PlaylistTrack;

namespace Chinook.Services;

public class TrackService
{
    private readonly IDbContextFactory<ChinookContext> _dbContextFactory;
    private readonly ChinookContext _dbContext;
    private const string Favorites = "Favorites";

    public TrackService(IDbContextFactory<ChinookContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task<List<PlaylistTrack>> GetTracksByArtistId(long artistId, string? userId)
    {
        using (var dbContext = await _dbContextFactory.CreateDbContextAsync())
        {
            return dbContext.Tracks.Where(a => a.Album.ArtistId == artistId)
            .Include(a => a.Album)
            .Include(p => p.Playlists)
            .Select(t => new PlaylistTrack()
            {
                AlbumTitle = (t.Album == null ? "-" : t.Album.Title),
                TrackId = t.TrackId,
                TrackName = t.Name,
                IsFavorite = t.Playlists.Where(p => p.UserPlaylists.Any(up => up.UserId == userId && up.Playlist.Name == Favorites)).Any(),
                Playlists = t.Playlists.Where(p => p.Name != Favorites).Select(p => new ClientModels.Playlist() {
                    Id = p.PlaylistId,
                    Name = p.Name
                }).ToList()
            })
            .ToList();
        }
    }

    public async Task UpdateFavoriteTrack(long trackId, string? userId, bool isFavorite = true)
    {
        UserPlaylist? userPlaylist = null;
        Playlist? playlist = null;
        Track? track = null;

        using (var dbContext = await _dbContextFactory.CreateDbContextAsync())
        {
            userPlaylist = dbContext
                .UserPlaylists
                .FirstOrDefault(up => up.UserId == userId && up.Playlist.Name == Favorites);

            playlist = dbContext
                .Playlists
                .Include(p => p.Tracks)
                .FirstOrDefault(u => u.Name == Favorites);


            track = dbContext.Tracks.FirstOrDefault(t => t.TrackId == trackId);
            
            if (userPlaylist == null)
            {
                playlist.Tracks.Add(track);

                userPlaylist = new()
                {
                    User = dbContext.Users.FirstOrDefault(u => u.Id == userId),
                    Playlist = playlist
                };

                dbContext.UserPlaylists.Add(userPlaylist);
            }
            else
            {
                if (isFavorite)
                    playlist.Tracks.Add(track);
                else
                    playlist.Tracks.Remove(track);

                dbContext.Update(playlist);
            }

            await dbContext.SaveChangesAsync();
        }
    }
}
