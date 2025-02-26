using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace moneygram_api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CodeTables",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CountryCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StateProvinceCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StateProvinceName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BaseCurrency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BaseCurrencyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LocalCurrency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LocalCurrencyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceiveCurrency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceiveCurrencyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IndicativeRateAvailable = table.Column<bool>(type: "bit", nullable: false),
                    DeliveryOption = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceiveAgentID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceiveAgentAbbreviation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MgManaged = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AgentManaged = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Version = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodeTables", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CountriesInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CountryCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CountryName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CountryLegacyCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SendActive = table.Column<bool>(type: "bit", nullable: false),
                    ReceiveActive = table.Column<bool>(type: "bit", nullable: false),
                    DirectedSendCountry = table.Column<bool>(type: "bit", nullable: false),
                    MgDirectedSendCountry = table.Column<bool>(type: "bit", nullable: false),
                    BaseReceiveCurrency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsZipCodeRequired = table.Column<bool>(type: "bit", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Emoji = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneLength = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Version = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CountriesInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CurrencyInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CurrencyCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CurrencyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CurrencyPrecision = table.Column<int>(type: "int", nullable: false),
                    Version = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExceptionLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExceptionMessage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InnerExceptionMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StackTrace = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HttpMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Origin = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExceptionLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RequestLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HttpMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Headers = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestBody = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResponseBody = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StatusCode = table.Column<int>(type: "int", nullable: false),
                    Device = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Origin = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ResponseTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestLogs", x => x.Id);
                });

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
                    ReceiveCurrency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReceiveAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SendCurrency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SendAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Charge = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalAmountCollected = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ExchangeRate = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    ConsumerID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Occupation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransactionPurpose = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SourceOfFunds = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FormFree = table.Column<bool>(type: "bit", nullable: true),
                    TellerId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    District = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Surburb = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Successful = table.Column<bool>(type: "bit", nullable: true),
                    Committed = table.Column<bool>(type: "bit", nullable: true),
                    CommitDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Processed = table.Column<bool>(type: "bit", nullable: true),
                    ProcessDate = table.Column<DateTime>(type: "datetime2", nullable: true)
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
                name: "CodeTables");

            migrationBuilder.DropTable(
                name: "CountriesInfo");

            migrationBuilder.DropTable(
                name: "CurrencyInfo");

            migrationBuilder.DropTable(
                name: "ExceptionLogs");

            migrationBuilder.DropTable(
                name: "RequestLogs");

            migrationBuilder.DropTable(
                name: "SendTransactions");
        }
    }
}
