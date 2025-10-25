using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace POS.Migrations
{
    /// <inheritdoc />
    public partial class New : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Clients_clientId",
                table: "Invoices");

            migrationBuilder.RenameColumn(
                name: "value",
                table: "Invoices",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "date",
                table: "Invoices",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "clientId",
                table: "Invoices",
                newName: "ClientId");

            migrationBuilder.RenameIndex(
                name: "IX_Invoices_clientId",
                table: "Invoices",
                newName: "IX_Invoices_ClientId");

            migrationBuilder.AddColumn<int>(
                name: "Number",
                table: "Invoices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "IdNumber",
                table: "Clients",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_Number",
                table: "Invoices",
                column: "Number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clients_IdNumber",
                table: "Clients",
                column: "IdNumber",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Clients_ClientId",
                table: "Invoices",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Clients_ClientId",
                table: "Invoices");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_Number",
                table: "Invoices");

            migrationBuilder.DropIndex(
                name: "IX_Clients_IdNumber",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "Number",
                table: "Invoices");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "Invoices",
                newName: "value");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Invoices",
                newName: "date");

            migrationBuilder.RenameColumn(
                name: "ClientId",
                table: "Invoices",
                newName: "clientId");

            migrationBuilder.RenameIndex(
                name: "IX_Invoices_ClientId",
                table: "Invoices",
                newName: "IX_Invoices_clientId");

            migrationBuilder.AlterColumn<string>(
                name: "IdNumber",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Clients_clientId",
                table: "Invoices",
                column: "clientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
