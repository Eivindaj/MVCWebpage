using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebappTest.Migrations
{
    public partial class HvemHvor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Periode",
                table: "stilling",
                newName: "Periode_Start");

            migrationBuilder.AddColumn<DateTime>(
                name: "Periode_Slutt",
                table: "stilling",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Periode_Slutt",
                table: "stilling");

            migrationBuilder.RenameColumn(
                name: "Periode_Start",
                table: "stilling",
                newName: "Periode");
        }
    }
}
