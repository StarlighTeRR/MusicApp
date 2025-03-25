using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicAppApi.Migrations
{
    /// <inheritdoc />
    public partial class AddLikesDislikesCounter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DislikesCount",
                table: "Tracks",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LikesCount",
                table: "Tracks",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DislikesCount",
                table: "Tracks");

            migrationBuilder.DropColumn(
                name: "LikesCount",
                table: "Tracks");
        }
    }
}
