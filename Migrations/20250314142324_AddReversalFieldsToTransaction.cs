using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace moneygram_api.Migrations
{
    /// <inheritdoc />
    public partial class AddReversalFieldsToTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReversalReason",
                table: "SendTransactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReversalTellerId",
                table: "SendTransactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReversalTime",
                table: "SendTransactions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Reversed",
                table: "SendTransactions",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReversalReason",
                table: "SendTransactions");

            migrationBuilder.DropColumn(
                name: "ReversalTellerId",
                table: "SendTransactions");

            migrationBuilder.DropColumn(
                name: "ReversalTime",
                table: "SendTransactions");

            migrationBuilder.DropColumn(
                name: "Reversed",
                table: "SendTransactions");
        }
    }
}
