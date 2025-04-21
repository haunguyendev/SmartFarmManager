using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartFarmManager.DataAccessObject.Migrations
{
    /// <inheritdoc />
    public partial class externalID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ExternalId",
                table: "Sensors",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ExternalId",
                table: "Farms",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ExternalId",
                table: "Cages",
                type: "uniqueidentifier",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExternalId",
                table: "Sensors");

            migrationBuilder.DropColumn(
                name: "ExternalId",
                table: "Farms");

            migrationBuilder.DropColumn(
                name: "ExternalId",
                table: "Cages");
        }
    }
}
