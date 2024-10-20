using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shop.WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class TotalAmount_is_changed_to_nullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalAmount",
                table: "Orders");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "TotalAmount",
                table: "Orders",
                type: "numeric",
                nullable: true);
        }
    }
}
