using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chinook.Migrations
{
    /// <inheritdoc />
    public partial class AddingdbsetPlayListTrack : Migration
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

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlaylistTrack",
                table: "PlaylistTrack");

            migrationBuilder.RenameTable(
                name: "PlaylistTrack",
                newName: "PlaylistTracks");

            migrationBuilder.RenameIndex(
                name: "IX_PlaylistTrack_TrackId",
                table: "PlaylistTracks",
                newName: "IX_PlaylistTracks_TrackId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlaylistTracks",
                table: "PlaylistTracks",
                columns: new[] { "PlaylistId", "TrackId" });

            migrationBuilder.AddForeignKey(
                name: "FK_PlaylistTracks_Playlist_PlaylistId",
                table: "PlaylistTracks",
                column: "PlaylistId",
                principalTable: "Playlist",
                principalColumn: "PlaylistId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlaylistTracks_Track_TrackId",
                table: "PlaylistTracks",
                column: "TrackId",
                principalTable: "Track",
                principalColumn: "TrackId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlaylistTracks_Playlist_PlaylistId",
                table: "PlaylistTracks");

            migrationBuilder.DropForeignKey(
                name: "FK_PlaylistTracks_Track_TrackId",
                table: "PlaylistTracks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlaylistTracks",
                table: "PlaylistTracks");

            migrationBuilder.RenameTable(
                name: "PlaylistTracks",
                newName: "PlaylistTrack");

            migrationBuilder.RenameIndex(
                name: "IX_PlaylistTracks_TrackId",
                table: "PlaylistTrack",
                newName: "IX_PlaylistTrack_TrackId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlaylistTrack",
                table: "PlaylistTrack",
                columns: new[] { "PlaylistId", "TrackId" });

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
    }
}
