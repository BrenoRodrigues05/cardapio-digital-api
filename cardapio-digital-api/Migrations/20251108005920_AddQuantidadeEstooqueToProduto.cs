using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cardapio_digital_api.Migrations
{
    /// <inheritdoc />
    public partial class AddQuantidadeEstooqueToProduto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuantidadeEstoque",
                table: "Produtos",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuantidadeEstoque",
                table: "Produtos");
        }
    }
}
