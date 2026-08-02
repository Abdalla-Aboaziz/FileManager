using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FileManager.Api.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RenameFileSizeInMBtoFileSize : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FileSizeInMB",
                table: "Files",
                newName: "FileSize");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FileSize",
                table: "Files",
                newName: "FileSizeInMB");
        }
    }
}
