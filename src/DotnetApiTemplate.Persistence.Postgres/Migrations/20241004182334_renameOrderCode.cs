using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotnetApiTemplate.Persistence.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class renameOrderCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CodeBooking",
                table: "TrBookingTicketBroker",
                newName: "OrderCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OrderCode",
                table: "TrBookingTicketBroker",
                newName: "CodeBooking");
        }
    }
}
