using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TopScore.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddWordConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_WordEntries_Word",
                table: "WordEntries",
                column: "Word",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_WordEntries_Word",
                table: "WordEntries");
        }
    }
}
