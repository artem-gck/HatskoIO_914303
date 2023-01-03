using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StructureService.Infrastructure.DataBase.Migrations
{
    public partial class ChangeNullableFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Departments_CheifUserId",
                table: "Departments");

            migrationBuilder.AlterColumn<Guid>(
                name: "CheifUserId",
                table: "Departments",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_CheifUserId",
                table: "Departments",
                column: "CheifUserId",
                unique: true,
                filter: "[CheifUserId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Departments_CheifUserId",
                table: "Departments");

            migrationBuilder.AlterColumn<Guid>(
                name: "CheifUserId",
                table: "Departments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Departments_CheifUserId",
                table: "Departments",
                column: "CheifUserId",
                unique: true);
        }
    }
}
