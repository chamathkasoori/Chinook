namespace Chinook.ClientModels;

public class Playlist
{
    public long Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public List<PlaylistTrack> Tracks { get; set; } = new();
}