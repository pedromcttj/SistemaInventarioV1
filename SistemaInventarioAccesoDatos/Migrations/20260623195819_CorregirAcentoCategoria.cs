using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaInventarioAccesoDatos.Migrations
{
    /// <inheritdoc />
    public partial class CorregirAcentoCategoria : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Descrípcion",
                table: "Categorias",
                newName: "Descripcion");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Descripcion",
                table: "Categorias",
                newName: "Descrípcion");
        }
    }
}
