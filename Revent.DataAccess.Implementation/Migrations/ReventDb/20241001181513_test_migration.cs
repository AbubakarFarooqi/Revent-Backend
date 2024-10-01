using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Revent.DataAccess.Implementation.Migrations.ReventDb
{
    /// <inheritdoc />
    public partial class test_migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Temp",
                columns: table => new
                {
                    column1 = table.Column<string>(type: "character varying", nullable: true)
                },
                constraints: table =>
                {
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Temp");
        }
    }
}
