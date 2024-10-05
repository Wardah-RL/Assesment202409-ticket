using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotnetApiTemplate.Persistence.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class addNoRekInPayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Nama",
                table: "TrPaymentBroker",
                newName: "NamaPengirim");

            migrationBuilder.RenameColumn(
                name: "Nama",
                table: "TrPayment",
                newName: "NamaPengirim");

            migrationBuilder.AddColumn<string>(
                name: "NoRekening",
                table: "TrPaymentBroker",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NoRekening",
                table: "TrPayment",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NoRekening",
                table: "TrPaymentBroker");

            migrationBuilder.DropColumn(
                name: "NoRekening",
                table: "TrPayment");

            migrationBuilder.RenameColumn(
                name: "NamaPengirim",
                table: "TrPaymentBroker",
                newName: "Nama");

            migrationBuilder.RenameColumn(
                name: "NamaPengirim",
                table: "TrPayment",
                newName: "Nama");
        }
    }
}
