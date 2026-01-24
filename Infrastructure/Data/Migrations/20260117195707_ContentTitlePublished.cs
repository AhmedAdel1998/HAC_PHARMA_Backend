using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HAC_Pharma.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class ContentTitlePublished : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPublished",
                table: "CmsContents",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "CmsContents",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPublished",
                table: "CmsContents");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "CmsContents");
        }
    }
}
