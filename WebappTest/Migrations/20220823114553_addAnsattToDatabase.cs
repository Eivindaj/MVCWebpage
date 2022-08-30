using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebappTest.Migrations
{
    public partial class addAnsattToDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ansatts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Navn = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ansatts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "oppgavers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Navn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ansatt_id = table.Column<int>(type: "int", nullable: false),
                    Dato = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_oppgavers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "stilling",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Navn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ansatt_id = table.Column<int>(type: "int", nullable: false),
                    Periode = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stilling", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ansatts");

            migrationBuilder.DropTable(
                name: "oppgavers");

            migrationBuilder.DropTable(
                name: "stilling");
        }
    }
}
