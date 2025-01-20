using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace moneygram_api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSendTransactionDecimalProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblMGSendTransactions");

            migrationBuilder.CreateTable(
                name: "SendTransactions",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SessionID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReferenceNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SenderAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OriginatingCountry = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Receiver = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceiverFirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceiverSurname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceiverMiddleName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceiverLastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceiverAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceiverCity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceiverCountry = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceiverState = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceiverZipCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceiverPhotoIDType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceiverPhotoIDNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceiverPhotoIDCountry = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceiverPhotoIDState = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceiverLegalIDType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceiverDOB = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Occupation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceiverCountryOfBirth = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceiverPhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConsumerID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransactionPurpose = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SourceOfFunds = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Charge = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FormFree = table.Column<bool>(type: "bit", nullable: false),
                    Maker = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    District = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Surburb = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AddDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Checker = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateAuthorised = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Successful = table.Column<bool>(type: "bit", nullable: false),
                    Commited = table.Column<bool>(type: "bit", nullable: false),
                    CommitDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Processed = table.Column<bool>(type: "bit", nullable: false),
                    ProcessDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Reversed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SendTransactions", x => x.ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SendTransactions");

            migrationBuilder.CreateTable(
                name: "tblMGSendTransactions",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AddDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Charge = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Checker = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CommitDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Commited = table.Column<bool>(type: "bit", nullable: false),
                    ConsumerID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateAuthorised = table.Column<DateTime>(type: "datetime2", nullable: true),
                    District = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FormFree = table.Column<bool>(type: "bit", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Maker = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Occupation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OriginatingCountry = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProcessDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Processed = table.Column<bool>(type: "bit", nullable: false),
                    Receiver = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceiverAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceiverCity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceiverCountry = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceiverCountryOfBirth = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceiverDOB = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceiverFirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceiverLastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceiverLegalIDType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceiverMiddleName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceiverPhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceiverPhotoIDCountry = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceiverPhotoIDNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceiverPhotoIDState = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceiverPhotoIDType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceiverState = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceiverSurname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceiverZipCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReferenceNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Reversed = table.Column<bool>(type: "bit", nullable: false),
                    Sender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SenderAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SessionID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SourceOfFunds = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Successful = table.Column<bool>(type: "bit", nullable: false),
                    Surburb = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransactionPurpose = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblMGSendTransactions", x => x.ID);
                });
        }
    }
}
