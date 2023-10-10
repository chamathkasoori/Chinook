namespace Chinook.Models;

public partial class Playlist
{
    public Playlist()
    {
        Tracks = new List<Track>();
    }

    public long PlaylistId { get; set; }

    public string? Name { get; set; }

    public virtual List<Track> Tracks { get; set; }

    public virtual ICollection<UserPlaylist> UserPlaylists { get; set; }

}
