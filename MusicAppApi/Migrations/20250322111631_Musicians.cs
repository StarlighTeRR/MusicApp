using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MusicAppApi.Migrations
{
    /// <inheritdoc />
    public partial class Musicians : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Albums_Artists_ArtistId",
                table: "Albums");

            migrationBuilder.DropForeignKey(
                name: "FK_Tracks_Albums_AlbumId",
                table: "Tracks");

            migrationBuilder.DropTable(
                name: "Artists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tracks",
                table: "Tracks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Albums",
                table: "Albums");

            migrationBuilder.RenameColumn(
                name: "TrackName",
                table: "Tracks",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "TrackId",
                table: "Tracks",
                newName: "Rating");

            migrationBuilder.RenameColumn(
                name: "AlbumId",
                table: "Albums",
                newName: "ReleaseYear");

            migrationBuilder.AlterColumn<int>(
                name: "Rating",
                table: "Tracks",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Tracks",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<bool>(
                name: "IsFavorite",
                table: "Tracks",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsListened",
                table: "Tracks",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "ReleaseYear",
                table: "Albums",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Albums",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tracks",
                table: "Tracks",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Albums",
                table: "Albums",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Musicians",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Age = table.Column<int>(type: "integer", nullable: false),
                    Genre = table.Column<string>(type: "text", nullable: false),
                    CareerStartYear = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Musicians", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Albums_Musicians_ArtistId",
                table: "Albums",
                column: "ArtistId",
                principalTable: "Musicians",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tracks_Albums_AlbumId",
                table: "Tracks",
                column: "AlbumId",
                principalTable: "Albums",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Albums_Musicians_ArtistId",
                table: "Albums");

            migrationBuilder.DropForeignKey(
                name: "FK_Tracks_Albums_AlbumId",
                table: "Tracks");

            migrationBuilder.DropTable(
                name: "Musicians");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tracks",
                table: "Tracks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Albums",
                table: "Albums");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Tracks");

            migrationBuilder.DropColumn(
                name: "IsFavorite",
                table: "Tracks");

            migrationBuilder.DropColumn(
                name: "IsListened",
                table: "Tracks");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Albums");

            migrationBuilder.RenameColumn(
                name: "Rating",
                table: "Tracks",
                newName: "TrackId");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Tracks",
                newName: "TrackName");

            migrationBuilder.RenameColumn(
                name: "ReleaseYear",
                table: "Albums",
                newName: "AlbumId");

            migrationBuilder.AlterColumn<int>(
                name: "TrackId",
                table: "Tracks",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "AlbumId",
                table: "Albums",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tracks",
                table: "Tracks",
                column: "TrackId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Albums",
                table: "Albums",
                column: "AlbumId");

            migrationBuilder.CreateTable(
                name: "Artists",
                columns: table => new
                {
                    ArtistId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artists", x => x.ArtistId);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Albums_Artists_ArtistId",
                table: "Albums",
                column: "ArtistId",
                principalTable: "Artists",
                principalColumn: "ArtistId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tracks_Albums_AlbumId",
                table: "Tracks",
                column: "AlbumId",
                principalTable: "Albums",
                principalColumn: "AlbumId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
