using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutomationSystem.Migrations
{
    /// <inheritdoc />
    public partial class Third : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AutomationSystemId",
                table: "Orders",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MasterId",
                table: "Orders",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_AutomationSystemId",
                table: "Orders",
                column: "AutomationSystemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_AutomationSystemId",
                table: "Orders",
                column: "AutomationSystemId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_AutomationSystemId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_AutomationSystemId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "AutomationSystemId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "MasterId",
                table: "Orders");
        }
    }
}
