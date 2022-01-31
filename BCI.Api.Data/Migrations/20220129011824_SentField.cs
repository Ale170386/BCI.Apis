using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BCI.Api.Data.Migrations
{
    public partial class SentField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Sent",
                table: "ActivationRequest",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sent",
                table: "ActivationRequest");
        }
    }
}
