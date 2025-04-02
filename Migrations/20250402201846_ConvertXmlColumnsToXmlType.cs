using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace moneygram_api.Migrations
{
    /// <inheritdoc />
   public partial class ConvertXmlColumnsToXmlType : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        // Step 1: Add temporary columns
        migrationBuilder.AddColumn<string>(
            name: "TempRequestXml",
            table: "MoneyGramXmlLogs",
            type: "xml",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "TempResponseXml",
            table: "MoneyGramXmlLogs",
            type: "xml",
            nullable: true);

        // Step 2: Clean and copy data
        migrationBuilder.Sql(@"
            UPDATE MoneyGramXmlLogs 
            SET TempRequestXml = 
                CASE 
                    WHEN TRY_CAST(RequestXml AS xml) IS NOT NULL 
                    THEN RequestXml
                    ELSE '<error>Invalid XML</error>'
                END,
                TempResponseXml = 
                CASE 
                    WHEN TRY_CAST(ResponseXml AS xml) IS NOT NULL 
                    THEN ResponseXml
                    ELSE '<error>Invalid XML</error>'
                END
        ");

        // Step 3: Drop original columns
        migrationBuilder.DropColumn(
            name: "RequestXml",
            table: "MoneyGramXmlLogs");

        migrationBuilder.DropColumn(
            name: "ResponseXml",
            table: "MoneyGramXmlLogs");

        // Step 4: Rename temporary columns
        migrationBuilder.RenameColumn(
            name: "TempRequestXml",
            table: "MoneyGramXmlLogs",
            newName: "RequestXml");

        migrationBuilder.RenameColumn(
            name: "TempResponseXml",
            table: "MoneyGramXmlLogs",
            newName: "ResponseXml");

        // Step 5: Make columns non-nullable
        migrationBuilder.AlterColumn<string>(
            name: "RequestXml",
            table: "MoneyGramXmlLogs",
            type: "xml",
            nullable: false,
            defaultValue: "<empty/>");

        migrationBuilder.AlterColumn<string>(
            name: "ResponseXml",
            table: "MoneyGramXmlLogs",
            type: "xml",
            nullable: false,
            defaultValue: "<empty/>");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        // Reverse operations if needed
    }
}
}
