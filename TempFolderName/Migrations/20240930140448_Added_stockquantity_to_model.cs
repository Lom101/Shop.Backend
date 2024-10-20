using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shop.WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class Added_stockquantity_to_model : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StockQuantity",
                table: "ModelSizes",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StockQuantity",
                table: "ModelSizes");
        }
    }
}
