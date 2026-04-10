using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeacherBreakApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class IntroduceSoftDeleteToModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "LeaveEntries",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "LeaveBalances",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "LeaveEntries");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "LeaveBalances");
        }
    }
}
