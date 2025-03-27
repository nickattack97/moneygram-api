using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace moneygram_api.Migrations
{
    /// <inheritdoc />
    public partial class AddAdditionalTransactionalFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReceiverLastName2",
                table: "SendTransactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReceiverPhotoIDExpiryDate",
                table: "SendTransactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RewardsNumber",
                table: "SendTransactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SenderLastName2",
                table: "SendTransactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SenderPhotoIDExpiryDate",
                table: "SendTransactions",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReceiverLastName2",
                table: "SendTransactions");

            migrationBuilder.DropColumn(
                name: "ReceiverPhotoIDExpiryDate",
                table: "SendTransactions");

            migrationBuilder.DropColumn(
                name: "RewardsNumber",
                table: "SendTransactions");

            migrationBuilder.DropColumn(
                name: "SenderLastName2",
                table: "SendTransactions");

            migrationBuilder.DropColumn(
                name: "SenderPhotoIDExpiryDate",
                table: "SendTransactions");
        }
    }
}
