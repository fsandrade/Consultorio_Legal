using Microsoft.EntityFrameworkCore.Migrations;

namespace CL.Data.Migrations
{
    public partial class addfuncao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Funcoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Funcoes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FuncaoUsuario",
                columns: table => new
                {
                    FuncoesId = table.Column<int>(type: "int", nullable: false),
                    UsuariosLogin = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FuncaoUsuario", x => new { x.FuncoesId, x.UsuariosLogin });
                    table.ForeignKey(
                        name: "FK_FuncaoUsuario_Funcoes_FuncoesId",
                        column: x => x.FuncoesId,
                        principalTable: "Funcoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FuncaoUsuario_Usuarios_UsuariosLogin",
                        column: x => x.UsuariosLogin,
                        principalTable: "Usuarios",
                        principalColumn: "Login",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FuncaoUsuario_UsuariosLogin",
                table: "FuncaoUsuario",
                column: "UsuariosLogin");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FuncaoUsuario");

            migrationBuilder.DropTable(
                name: "Funcoes");
        }
    }
}
