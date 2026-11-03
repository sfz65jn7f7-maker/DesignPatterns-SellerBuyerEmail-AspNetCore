using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InscriptionApp.Api.Migrations
{
    /// <inheritdoc />
    public partial class AjoutLangue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Langue",
                table: "Vendeurs",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: false,
                defaultValue: "Fr");

            migrationBuilder.AddColumn<string>(
                name: "Langue",
                table: "Acheteurs",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: false,
                defaultValue: "Fr");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Langue",
                table: "Vendeurs");

            migrationBuilder.DropColumn(
                name: "Langue",
                table: "Acheteurs");
        }
    }
}
