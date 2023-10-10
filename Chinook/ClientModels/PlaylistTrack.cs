namespace Chinook.ClientModels;

public class PlaylistTrack
{
    public long TrackId { get; set; }

    public string TrackName { get; set; } = string.Empty;

    public string AlbumTitle { get; set; } = string.Empty;

    public string ArtistName { get; set; } = string.Empty;

    public bool IsFavorite { get; set; }

    public List<Playlist> Playlists { get; set; }
}