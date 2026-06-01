using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_PostLikes_UserId_PostId",
                table: "PostLikes",
                columns: new[] { "UserId", "PostId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PostLikes_UserId_PostId",
                table: "PostLikes");
        }
    }
}
