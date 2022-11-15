using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LivrEtec.Migrations
{
    public partial class UpdateLivrosAutor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Autores_Livros_Livrocd",
                table: "Autores");

            migrationBuilder.DropIndex(
                name: "IX_Autores_Livrocd",
                table: "Autores");

            migrationBuilder.DropColumn(
                name: "Livrocd",
                table: "Autores");

            migrationBuilder.CreateTable(
                name: "AutorLivro",
                columns: table => new
                {
                    Autorescd = table.Column<int>(type: "int", nullable: false),
                    Livroscd = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AutorLivro", x => new { x.Autorescd, x.Livroscd });
                    table.ForeignKey(
                        name: "FK_AutorLivro_Autores_Autorescd",
                        column: x => x.Autorescd,
                        principalTable: "Autores",
                        principalColumn: "cd",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AutorLivro_Livros_Livroscd",
                        column: x => x.Livroscd,
                        principalTable: "Livros",
                        principalColumn: "cd",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_AutorLivro_Livroscd",
                table: "AutorLivro",
                column: "Livroscd");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AutorLivro");

            migrationBuilder.AddColumn<int>(
                name: "Livrocd",
                table: "Autores",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Autores_Livrocd",
                table: "Autores",
                column: "Livrocd");

            migrationBuilder.AddForeignKey(
                name: "FK_Autores_Livros_Livrocd",
                table: "Autores",
                column: "Livrocd",
                principalTable: "Livros",
                principalColumn: "cd");
        }
    }
}
