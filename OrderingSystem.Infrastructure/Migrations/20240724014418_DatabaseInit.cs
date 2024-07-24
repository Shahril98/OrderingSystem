using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderingSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DatabaseInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_tbl_menu_group_name",
                table: "tbl_menu_group");

            migrationBuilder.DropIndex(
                name: "ix_tbl_menu_name",
                table: "tbl_menu");

            migrationBuilder.CreateIndex(
                name: "ix_tbl_menu_group_name",
                table: "tbl_menu_group",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_tbl_menu_name",
                table: "tbl_menu",
                column: "name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_tbl_menu_group_name",
                table: "tbl_menu_group");

            migrationBuilder.DropIndex(
                name: "ix_tbl_menu_name",
                table: "tbl_menu");

            migrationBuilder.CreateIndex(
                name: "ix_tbl_menu_group_name",
                table: "tbl_menu_group",
                column: "name",
                unique: true,
                filter: "is_deleted = false");

            migrationBuilder.CreateIndex(
                name: "ix_tbl_menu_name",
                table: "tbl_menu",
                column: "name",
                unique: true,
                filter: "is_deleted = false");
        }
    }
}
