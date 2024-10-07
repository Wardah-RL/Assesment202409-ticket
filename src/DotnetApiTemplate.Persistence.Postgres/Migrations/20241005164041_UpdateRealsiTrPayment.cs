using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotnetApiTemplate.Persistence.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRealsiTrPayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrPayment_TrBookingTicketBroker_IdBookingTicket",
                table: "TrPayment");

            migrationBuilder.AddForeignKey(
                name: "FK_TrPayment_TrBookingTicket_IdBookingTicket",
                table: "TrPayment",
                column: "IdBookingTicket",
                principalTable: "TrBookingTicket",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrPayment_TrBookingTicket_IdBookingTicket",
                table: "TrPayment");

            migrationBuilder.AddForeignKey(
                name: "FK_TrPayment_TrBookingTicketBroker_IdBookingTicket",
                table: "TrPayment",
                column: "IdBookingTicket",
                principalTable: "TrBookingTicketBroker",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
