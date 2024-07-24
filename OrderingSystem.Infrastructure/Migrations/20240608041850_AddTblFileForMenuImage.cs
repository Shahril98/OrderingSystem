using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderingSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTblFileForMenuImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_file",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    timestamp_created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    timestamp_updated = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    extension = table.Column<string>(type: "text", nullable: false),
                    data = table.Column<byte[]>(type: "bytea", maxLength: 1100000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_file", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_menu_image",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    timestamp_created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    timestamp_updated = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    menu_id = table.Column<Guid>(type: "uuid", nullable: false),
                    file_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_menu_image", x => x.id);
                    table.ForeignKey(
                        name: "fk_tbl_menu_image_tbl_file_file_id",
                        column: x => x.file_id,
                        principalTable: "tbl_file",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_tbl_menu_image_tbl_menu_menu_id",
                        column: x => x.menu_id,
                        principalTable: "tbl_menu",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "ix_tbl_menu_image_file_id",
                table: "tbl_menu_image",
                column: "file_id");

            migrationBuilder.CreateIndex(
                name: "ix_tbl_menu_image_menu_id",
                table: "tbl_menu_image",
                column: "menu_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_menu_image");

            migrationBuilder.DropTable(
                name: "tbl_file");
        }
    }
}
