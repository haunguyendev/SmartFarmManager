using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartFarmManager.DataAccessObject.Migrations
{
    /// <inheritdoc />
    public partial class tableDeadPou : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DeadPoultryLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    FarmingBatchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeadPoultryLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeadPoultryLogs_FarmingBatchs_FarmingBatchId",
                        column: x => x.FarmingBatchId,
                        principalTable: "FarmingBatchs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeadPoultryLogs_FarmingBatchId",
                table: "DeadPoultryLogs",
                column: "FarmingBatchId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeadPoultryLogs");
        }
    }
}
