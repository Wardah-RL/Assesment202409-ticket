using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotnetApiTemplate.Persistence.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class addPayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Note",
                table: "TrBookingTicketBroker",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(512)",
                oldMaxLength: 512,
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CodeBooking",
                table: "TrBookingTicketBroker",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateEvent",
                table: "TrBookingTicketBroker",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateEvent",
                table: "TrBookingTicket",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "MsBank",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
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
                    table.PrimaryKey("PK_MsBank", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrPayment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IdBookingTicket = table.Column<Guid>(type: "uuid", nullable: false),
                    TotalPayment = table.Column<int>(type: "integer", nullable: false),
                    Nama = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Bank = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
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
                        name: "FK_TrPayment_TrBookingTicketBroker_IdBookingTicket",
                        column: x => x.IdBookingTicket,
                        principalTable: "TrBookingTicketBroker",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrPaymentBroker",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IdBookingTicketBroker = table.Column<Guid>(type: "uuid", nullable: false),
                    IdBank = table.Column<Guid>(type: "uuid", nullable: false),
                    TotalPayment = table.Column<int>(type: "integer", nullable: false),
                    Nama = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
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
                    table.PrimaryKey("PK_TrPaymentBroker", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrPaymentBroker_MsBank_IdBank",
                        column: x => x.IdBank,
                        principalTable: "MsBank",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrPaymentBroker_TrBookingTicketBroker_IdBookingTicketBroker",
                        column: x => x.IdBookingTicketBroker,
                        principalTable: "TrBookingTicketBroker",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MsBank_CreatedByFullName",
                table: "MsBank",
                column: "CreatedByFullName");

            migrationBuilder.CreateIndex(
                name: "IX_MsBank_CreatedByName",
                table: "MsBank",
                column: "CreatedByName");

            migrationBuilder.CreateIndex(
                name: "IX_MsBank_Id",
                table: "MsBank",
                column: "Id")
                .Annotation("Npgsql:IndexMethod", "hash");

            migrationBuilder.CreateIndex(
                name: "IX_MsBank_LastUpdatedBy",
                table: "MsBank",
                column: "LastUpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MsBank_LastUpdatedByFullName",
                table: "MsBank",
                column: "LastUpdatedByFullName");

            migrationBuilder.CreateIndex(
                name: "IX_MsBank_LastUpdatedByName",
                table: "MsBank",
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

            migrationBuilder.CreateIndex(
                name: "IX_TrPaymentBroker_CreatedByFullName",
                table: "TrPaymentBroker",
                column: "CreatedByFullName");

            migrationBuilder.CreateIndex(
                name: "IX_TrPaymentBroker_CreatedByName",
                table: "TrPaymentBroker",
                column: "CreatedByName");

            migrationBuilder.CreateIndex(
                name: "IX_TrPaymentBroker_Id",
                table: "TrPaymentBroker",
                column: "Id")
                .Annotation("Npgsql:IndexMethod", "hash");

            migrationBuilder.CreateIndex(
                name: "IX_TrPaymentBroker_IdBank",
                table: "TrPaymentBroker",
                column: "IdBank");

            migrationBuilder.CreateIndex(
                name: "IX_TrPaymentBroker_IdBookingTicketBroker",
                table: "TrPaymentBroker",
                column: "IdBookingTicketBroker");

            migrationBuilder.CreateIndex(
                name: "IX_TrPaymentBroker_LastUpdatedBy",
                table: "TrPaymentBroker",
                column: "LastUpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_TrPaymentBroker_LastUpdatedByFullName",
                table: "TrPaymentBroker",
                column: "LastUpdatedByFullName");

            migrationBuilder.CreateIndex(
                name: "IX_TrPaymentBroker_LastUpdatedByName",
                table: "TrPaymentBroker",
                column: "LastUpdatedByName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrPayment");

            migrationBuilder.DropTable(
                name: "TrPaymentBroker");

            migrationBuilder.DropTable(
                name: "MsBank");

            migrationBuilder.DropColumn(
                name: "CodeBooking",
                table: "TrBookingTicketBroker");

            migrationBuilder.DropColumn(
                name: "DateEvent",
                table: "TrBookingTicketBroker");

            migrationBuilder.DropColumn(
                name: "DateEvent",
                table: "TrBookingTicket");

            migrationBuilder.AlterColumn<string>(
                name: "Note",
                table: "TrBookingTicketBroker",
                type: "character varying(512)",
                maxLength: 512,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldNullable: true);
        }
    }
}
