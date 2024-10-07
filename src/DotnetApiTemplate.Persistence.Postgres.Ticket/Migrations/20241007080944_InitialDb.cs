using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotnetApiTemplate.Persistence.Postgres.Ticket.Migrations
{
    /// <inheritdoc />
    public partial class InitialDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MsEvent",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CountTicket = table.Column<int>(type: "integer", nullable: false),
                    Location = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Price = table.Column<int>(type: "integer", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    CreatedByName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    CreatedByFullName = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAtServer = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastUpdatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    LastUpdatedByName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    LastUpdatedByFullName = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastUpdatedAtServer = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    StatusRecord = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MsEvent", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrBookingTicket",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IdBookingTicketBroker = table.Column<Guid>(type: "uuid", nullable: false),
                    IdEvent = table.Column<Guid>(type: "uuid", nullable: false),
                    FullName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    CountTicket = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    DateEvent = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    CreatedByName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    CreatedByFullName = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAtServer = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastUpdatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    LastUpdatedByName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    LastUpdatedByFullName = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastUpdatedAtServer = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    StatusRecord = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrBookingTicket", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrBookingTicket_MsEvent_IdEvent",
                        column: x => x.IdEvent,
                        principalTable: "MsEvent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrPayment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IdBookingTicket = table.Column<Guid>(type: "uuid", nullable: false),
                    TotalPayment = table.Column<int>(type: "integer", nullable: false),
                    NamaPengirim = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Bank = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    NoRekening = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    CreatedByName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    CreatedByFullName = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAtServer = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastUpdatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    LastUpdatedByName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    LastUpdatedByFullName = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastUpdatedAtServer = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    StatusRecord = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrPayment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrPayment_TrBookingTicket_IdBookingTicket",
                        column: x => x.IdBookingTicket,
                        principalTable: "TrBookingTicket",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MsEvent_CreatedByFullName",
                table: "MsEvent",
                column: "CreatedByFullName");

            migrationBuilder.CreateIndex(
                name: "IX_MsEvent_CreatedByName",
                table: "MsEvent",
                column: "CreatedByName");

            migrationBuilder.CreateIndex(
                name: "IX_MsEvent_Id",
                table: "MsEvent",
                column: "Id")
                .Annotation("Npgsql:IndexMethod", "hash");

            migrationBuilder.CreateIndex(
                name: "IX_MsEvent_LastUpdatedBy",
                table: "MsEvent",
                column: "LastUpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MsEvent_LastUpdatedByFullName",
                table: "MsEvent",
                column: "LastUpdatedByFullName");

            migrationBuilder.CreateIndex(
                name: "IX_MsEvent_LastUpdatedByName",
                table: "MsEvent",
                column: "LastUpdatedByName");

            migrationBuilder.CreateIndex(
                name: "IX_TrBookingTicket_CreatedByFullName",
                table: "TrBookingTicket",
                column: "CreatedByFullName");

            migrationBuilder.CreateIndex(
                name: "IX_TrBookingTicket_CreatedByName",
                table: "TrBookingTicket",
                column: "CreatedByName");

            migrationBuilder.CreateIndex(
                name: "IX_TrBookingTicket_Id",
                table: "TrBookingTicket",
                column: "Id")
                .Annotation("Npgsql:IndexMethod", "hash");

            migrationBuilder.CreateIndex(
                name: "IX_TrBookingTicket_IdEvent",
                table: "TrBookingTicket",
                column: "IdEvent");

            migrationBuilder.CreateIndex(
                name: "IX_TrBookingTicket_LastUpdatedBy",
                table: "TrBookingTicket",
                column: "LastUpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_TrBookingTicket_LastUpdatedByFullName",
                table: "TrBookingTicket",
                column: "LastUpdatedByFullName");

            migrationBuilder.CreateIndex(
                name: "IX_TrBookingTicket_LastUpdatedByName",
                table: "TrBookingTicket",
                column: "LastUpdatedByName");

            migrationBuilder.CreateIndex(
                name: "IX_TrPayment_CreatedByFullName",
                table: "TrPayment",
                column: "CreatedByFullName");

            migrationBuilder.CreateIndex(
                name: "IX_TrPayment_CreatedByName",
                table: "TrPayment",
                column: "CreatedByName");

            migrationBuilder.CreateIndex(
                name: "IX_TrPayment_Id",
                table: "TrPayment",
                column: "Id")
                .Annotation("Npgsql:IndexMethod", "hash");

            migrationBuilder.CreateIndex(
                name: "IX_TrPayment_IdBookingTicket",
                table: "TrPayment",
                column: "IdBookingTicket");

            migrationBuilder.CreateIndex(
                name: "IX_TrPayment_LastUpdatedBy",
                table: "TrPayment",
                column: "LastUpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_TrPayment_LastUpdatedByFullName",
                table: "TrPayment",
                column: "LastUpdatedByFullName");

            migrationBuilder.CreateIndex(
                name: "IX_TrPayment_LastUpdatedByName",
                table: "TrPayment",
                column: "LastUpdatedByName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrPayment");

            migrationBuilder.DropTable(
                name: "TrBookingTicket");

            migrationBuilder.DropTable(
                name: "MsEvent");
        }
    }
}
