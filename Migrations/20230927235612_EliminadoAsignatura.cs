using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppNano.Migrations
{
    public partial class EliminadoAsignatura : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Eliminado",
                table: "Asignaturas",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Eliminado",
                table: "Asignaturas");
        }
    }
}
