using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotnetApiTemplate.Persistence.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class removeRelasiEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrBookingTicketBroker_MsEvent_MsEventId",
                table: "TrBookingTicketBroker");

            migrationBuilder.DropIndex(
                name: "IX_TrBookingTicketBroker_MsEventId",
                table: "TrBookingTicketBroker");

            migrationBuilder.DropColumn(
                name: "MsEventId",
                table: "TrBookingTicketBroker");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "MsEventId",
                table: "TrBookingTicketBroker",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrBookingTicketBroker_MsEventId",
                table: "TrBookingTicketBroker",
                column: "MsEventId");

            migrationBuilder.AddForeignKey(
                name: "FK_TrBookingTicketBroker_MsEvent_MsEventId",
                table: "TrBookingTicketBroker",
                column: "MsEventId",
                principalTable: "MsEvent",
                principalColumn: "Id");
        }
    }
}
