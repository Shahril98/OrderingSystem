using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderingSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateIndexMenuGroup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_tbl_menu_group_name",
                table: "tbl_menu_group");

            migrationBuilder.CreateIndex(
                name: "ix_tbl_menu_group_name",
                table: "tbl_menu_group",
                column: "name",
                unique: true,
                filter: "is_deleted = false");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_tbl_menu_group_name",
                table: "tbl_menu_group");

            migrationBuilder.CreateIndex(
                name: "ix_tbl_menu_group_name",
                table: "tbl_menu_group",
                column: "name",
                unique: true);
        }
    }
}
