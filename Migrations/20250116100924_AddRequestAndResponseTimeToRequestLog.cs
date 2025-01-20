using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace moneygram_api.Migrations
{
    /// <inheritdoc />
    public partial class AddRequestAndResponseTimeToRequestLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Timestamp",
                table: "RequestLogs",
                newName: "RequestTime");

            migrationBuilder.AddColumn<DateTime>(
                name: "ResponseTime",
                table: "RequestLogs",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResponseTime",
                table: "RequestLogs");

            migrationBuilder.RenameColumn(
                name: "RequestTime",
                table: "RequestLogs",
                newName: "Timestamp");
        }
    }
}
