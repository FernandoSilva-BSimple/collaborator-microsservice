using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EmailAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "CollaboratorsTemp",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "FinalDate",
                table: "CollaboratorsTemp",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Names",
                table: "CollaboratorsTemp",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Surnames",
                table: "CollaboratorsTemp",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "CollaboratorsTemp");

            migrationBuilder.DropColumn(
                name: "FinalDate",
                table: "CollaboratorsTemp");

            migrationBuilder.DropColumn(
                name: "Names",
                table: "CollaboratorsTemp");

            migrationBuilder.DropColumn(
                name: "Surnames",
                table: "CollaboratorsTemp");
        }
    }
}
