using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chinook.Migrations
{
    /// <inheritdoc />
    public partial class AddingPlayListTrack : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlaylistTrack_Playlist_PlaylistId",
                table: "PlaylistTrack");

            migrationBuilder.DropForeignKey(
                name: "FK_PlaylistTrack_Track_TrackId",
                table: "PlaylistTrack");

            migrationBuilder.RenameIndex(
                name: "IFK_PlaylistTrackTrackId",
                table: "PlaylistTrack",
                newName: "IX_PlaylistTrack_TrackId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlaylistTrack_Playlist_PlaylistId",
                table: "PlaylistTrack",
                column: "PlaylistId",
                principalTable: "Playlist",
                principalColumn: "PlaylistId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlaylistTrack_Track_TrackId",
                table: "PlaylistTrack",
                column: "TrackId",
                principalTable: "Track",
                principalColumn: "TrackId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlaylistTrack_Playlist_PlaylistId",
                table: "PlaylistTrack");

            migrationBuilder.DropForeignKey(
                name: "FK_PlaylistTrack_Track_TrackId",
                table: "PlaylistTrack");

            migrationBuilder.RenameIndex(
                name: "IX_PlaylistTrack_TrackId",
                table: "PlaylistTrack",
                newName: "IFK_PlaylistTrackTrackId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlaylistTrack_Playlist_PlaylistId",
                table: "PlaylistTrack",
                column: "PlaylistId",
                principalTable: "Playlist",
                principalColumn: "PlaylistId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlaylistTrack_Track_TrackId",
                table: "PlaylistTrack",
                column: "TrackId",
                principalTable: "Track",
                principalColumn: "TrackId");
        }
    }
}
