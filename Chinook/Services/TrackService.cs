using Chinook.Data;
using Chinook.Models;
using Microsoft.EntityFrameworkCore;
using PlaylistTrack = Chinook.ClientModels.PlaylistTrack;

namespace Chinook.Services;

public class TrackService
{
    private readonly IDbContextFactory<ChinookContext> _dbContextFactory;
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
            .Select(t => new PlaylistTrack()
            {
                AlbumTitle = (t.Album == null ? "-" : t.Album.Title),
                TrackId = t.TrackId,
                TrackName = t.Name,
                IsFavorite = t.Playlists.Where(p => p.UserPlaylists.Any(up => up.UserId == userId && up.Playlist.Name == Favorites)).Any()
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
                .FirstOrDefault(u => u.Name == Favorites);

            track = dbContext.Tracks.FirstOrDefault(t => t.TrackId == trackId);

            if (userPlaylist == null)
            {
                userPlaylist = new()
                {
                    User = dbContext.Users.FirstOrDefault(u => u.Id == userId),
                    Playlist = playlist
                };

                dbContext.UserPlaylists.Add(userPlaylist);
            }

            if (isFavorite)
            {
                Models.PlaylistTrack playlistTrack = new()
                {
                    Playlist = playlist,
                    Track = track
                };

                dbContext.PlaylistTracks.Add(playlistTrack);
            }
            else
            {
                var playLisTrack = dbContext
                    .PlaylistTracks
                    .FirstOrDefault(t => t.Playlist == playlist && t.Track == track);

                dbContext.PlaylistTracks.Remove(playLisTrack);
            }

            await dbContext.SaveChangesAsync();
        }
    }
}
