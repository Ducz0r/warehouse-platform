using Microsoft.EntityFrameworkCore.Migrations;

namespace Warehouse.Server.Data.Migrations
{
    public partial class AddPasswordSaltToCustomers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "PasswordSalt",
                table: "Customers",
                type: "BLOB",
                nullable: false,
                defaultValue: new byte[128 / 8]);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordSalt",
                table: "Customers");
        }
    }
}
