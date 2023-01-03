using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskCrudService.Adapters.DataSource.Migrations
{
    public partial class AddDocumentIdField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SignatureDocumentId",
                table: "Performers",
                type: "uniqueidentifier",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SignatureDocumentId",
                table: "Performers");
        }
    }
}
