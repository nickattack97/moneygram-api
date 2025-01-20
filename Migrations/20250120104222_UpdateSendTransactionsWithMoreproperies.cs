using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace moneygram_api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSendTransactionsWithMoreproperies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateAuthorised",
                table: "SendTransactions");

            migrationBuilder.DropColumn(
                name: "Reversed",
                table: "SendTransactions");

            migrationBuilder.RenameColumn(
                name: "State",
                table: "SendTransactions",
                newName: "TellerId");

            migrationBuilder.RenameColumn(
                name: "SenderAddress",
                table: "SendTransactions",
                newName: "SenderZipCode");

            migrationBuilder.RenameColumn(
                name: "ReceiverSurname",
                table: "SendTransactions",
                newName: "SenderState");

            migrationBuilder.RenameColumn(
                name: "ReceiverAddress",
                table: "SendTransactions",
                newName: "SenderPhotoIDType");

            migrationBuilder.RenameColumn(
                name: "Message",
                table: "SendTransactions",
                newName: "SenderPhotoIDState");

            migrationBuilder.RenameColumn(
                name: "Maker",
                table: "SendTransactions",
                newName: "SenderPhotoIDNumber");

            migrationBuilder.RenameColumn(
                name: "Gender",
                table: "SendTransactions",
                newName: "SenderPhotoIDCountry");

            migrationBuilder.RenameColumn(
                name: "Currency",
                table: "SendTransactions",
                newName: "SenderPhoneNumber");

            migrationBuilder.AddColumn<string>(
                name: "ReceiveCurrency",
                table: "SendTransactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReceiverAddress1",
                table: "SendTransactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReceiverAddress2",
                table: "SendTransactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReceiverAddress3",
                table: "SendTransactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReceiverGender",
                table: "SendTransactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SendCurrency",
                table: "SendTransactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SenderAddress1",
                table: "SendTransactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SenderAddress2",
                table: "SendTransactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SenderAddress3",
                table: "SendTransactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SenderCity",
                table: "SendTransactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SenderCountry",
                table: "SendTransactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SenderCountryOfBirth",
                table: "SendTransactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SenderDOB",
                table: "SendTransactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SenderFirstName",
                table: "SendTransactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SenderGender",
                table: "SendTransactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SenderLastName",
                table: "SendTransactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SenderLegalIDType",
                table: "SendTransactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SenderMiddleName",
                table: "SendTransactions",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReceiveCurrency",
                table: "SendTransactions");

            migrationBuilder.DropColumn(
                name: "ReceiverAddress1",
                table: "SendTransactions");

            migrationBuilder.DropColumn(
                name: "ReceiverAddress2",
                table: "SendTransactions");

            migrationBuilder.DropColumn(
                name: "ReceiverAddress3",
                table: "SendTransactions");

            migrationBuilder.DropColumn(
                name: "ReceiverGender",
                table: "SendTransactions");

            migrationBuilder.DropColumn(
                name: "SendCurrency",
                table: "SendTransactions");

            migrationBuilder.DropColumn(
                name: "SenderAddress1",
                table: "SendTransactions");

            migrationBuilder.DropColumn(
                name: "SenderAddress2",
                table: "SendTransactions");

            migrationBuilder.DropColumn(
                name: "SenderAddress3",
                table: "SendTransactions");

            migrationBuilder.DropColumn(
                name: "SenderCity",
                table: "SendTransactions");

            migrationBuilder.DropColumn(
                name: "SenderCountry",
                table: "SendTransactions");

            migrationBuilder.DropColumn(
                name: "SenderCountryOfBirth",
                table: "SendTransactions");

            migrationBuilder.DropColumn(
                name: "SenderDOB",
                table: "SendTransactions");

            migrationBuilder.DropColumn(
                name: "SenderFirstName",
                table: "SendTransactions");

            migrationBuilder.DropColumn(
                name: "SenderGender",
                table: "SendTransactions");

            migrationBuilder.DropColumn(
                name: "SenderLastName",
                table: "SendTransactions");

            migrationBuilder.DropColumn(
                name: "SenderLegalIDType",
                table: "SendTransactions");

            migrationBuilder.DropColumn(
                name: "SenderMiddleName",
                table: "SendTransactions");

            migrationBuilder.RenameColumn(
                name: "TellerId",
                table: "SendTransactions",
                newName: "State");

            migrationBuilder.RenameColumn(
                name: "SenderZipCode",
                table: "SendTransactions",
                newName: "SenderAddress");

            migrationBuilder.RenameColumn(
                name: "SenderState",
                table: "SendTransactions",
                newName: "ReceiverSurname");

            migrationBuilder.RenameColumn(
                name: "SenderPhotoIDType",
                table: "SendTransactions",
                newName: "ReceiverAddress");

            migrationBuilder.RenameColumn(
                name: "SenderPhotoIDState",
                table: "SendTransactions",
                newName: "Message");

            migrationBuilder.RenameColumn(
                name: "SenderPhotoIDNumber",
                table: "SendTransactions",
                newName: "Maker");

            migrationBuilder.RenameColumn(
                name: "SenderPhotoIDCountry",
                table: "SendTransactions",
                newName: "Gender");

            migrationBuilder.RenameColumn(
                name: "SenderPhoneNumber",
                table: "SendTransactions",
                newName: "Currency");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateAuthorised",
                table: "SendTransactions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Reversed",
                table: "SendTransactions",
                type: "bit",
                nullable: true);
        }
    }
}
