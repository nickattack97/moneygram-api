using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace moneygram_api.Migrations
{
    /// <inheritdoc />
    public partial class AddSendTransactionsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Checker",
                table: "SendTransactions");

            migrationBuilder.RenameColumn(
                name: "Commited",
                table: "SendTransactions",
                newName: "Committed");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "SendTransactions",
                newName: "SendAmount");

            migrationBuilder.AddColumn<decimal>(
                name: "ExchangeRate",
                table: "SendTransactions",
                type: "decimal(18,4)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ReceiveAmount",
                table: "SendTransactions",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExchangeRate",
                table: "SendTransactions");

            migrationBuilder.DropColumn(
                name: "ReceiveAmount",
                table: "SendTransactions");

            migrationBuilder.RenameColumn(
                name: "SendAmount",
                table: "SendTransactions",
                newName: "Amount");

            migrationBuilder.RenameColumn(
                name: "Committed",
                table: "SendTransactions",
                newName: "Commited");

            migrationBuilder.AddColumn<string>(
                name: "Checker",
                table: "SendTransactions",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
