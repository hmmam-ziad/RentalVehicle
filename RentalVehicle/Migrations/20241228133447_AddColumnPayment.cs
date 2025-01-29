using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentalVehicle.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnPayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PaymentDetails",
                table: "RentalTransactions",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentDetails",
                table: "RentalTransactions");
        }
    }
}
