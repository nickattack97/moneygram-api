using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace moneygram_api.Migrations
{
    /// <inheritdoc />
   public partial class CreateSendTransactionsTable : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "SendTransactions",
            columns: table => new
            {
                ID = table.Column<long>(type: "bigint", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                SessionID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ReferenceNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Sender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                SenderFirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                SenderMiddleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                SenderLastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                SenderAddress1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                SenderAddress2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                SenderAddress3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                SenderCity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                SenderCountry = table.Column<string>(type: "nvarchar(max)", nullable: true),
                SenderState = table.Column<string>(type: "nvarchar(max)", nullable: true),
                SenderZipCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                SenderPhotoIDType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                SenderPhotoIDNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                SenderPhotoIDCountry = table.Column<string>(type: "nvarchar(max)", nullable: true),
                SenderPhotoIDState = table.Column<string>(type: "nvarchar(max)", nullable: true),
                SenderLegalIDType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                SenderGender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                SenderDOB = table.Column<string>(type: "nvarchar(max)", nullable: true),
                SenderCountryOfBirth = table.Column<string>(type: "nvarchar(max)", nullable: true),
                SenderPhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                OriginatingCountry = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Receiver = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ReceiverFirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ReceiverMiddleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ReceiverLastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ReceiverAddress1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ReceiverAddress2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ReceiverAddress3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ReceiverCity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ReceiverCountry = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ReceiverState = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ReceiverZipCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ReceiverPhotoIDType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ReceiverPhotoIDNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ReceiverPhotoIDCountry = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ReceiverPhotoIDState = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ReceiverLegalIDType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ReceiverGender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ReceiverDOB = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ReceiverCountryOfBirth = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ReceiverPhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                SendCurrency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ReceiveCurrency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                Charge = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                ConsumerID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Occupation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                TransactionPurpose = table.Column<string>(type: "nvarchar(max)", nullable: true),
                SourceOfFunds = table.Column<string>(type: "nvarchar(max)", nullable: true),
                FormFree = table.Column<bool>(type: "bit", nullable: true),
                TellerId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                District = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Surburb = table.Column<string>(type: "nvarchar(max)", nullable: true),
                AddDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                Checker = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Successful = table.Column<bool>(type: "bit", nullable: true),
                Commited = table.Column<bool>(type: "bit", nullable: true),
                CommitDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                Processed = table.Column<bool>(type: "bit", nullable: true),
                ProcessDate = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_SendTransactions", x => x.ID);
            });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "SendTransactions");
    }
}
}
