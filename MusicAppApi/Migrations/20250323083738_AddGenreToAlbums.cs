using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicAppApi.Migrations
{
    /// <inheritdoc />
    public partial class AddGenreToAlbums : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReleaseYear",
                table: "Albums");

            migrationBuilder.RenameColumn(
                name: "AlbumName",
                table: "Albums",
                newName: "Genre");

            migrationBuilder.AddColumn<DateTime>(
                name: "ReleaseDate",
                table: "Albums",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Albums",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReleaseDate",
                table: "Albums");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Albums");

            migrationBuilder.RenameColumn(
                name: "Genre",
                table: "Albums",
                newName: "AlbumName");

            migrationBuilder.AddColumn<int>(
                name: "ReleaseYear",
                table: "Albums",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
