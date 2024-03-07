using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutomationSystem.Migrations
{
    /// <inheritdoc />
    public partial class addedMasterDatas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MasterDatas",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    isNewOrdersChecked = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterDatas", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MasterDatas");
        }
    }
}
