using Microsoft.EntityFrameworkCore.Migrations;
using OrderingSystem.Infrastructure.Databases.OrderingSystem;

#nullable disable

namespace OrderingSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMenuTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_active",
                table: "tbl_menu");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:entity_state", "detached,unchanged,deleted,modified,added")
                .Annotation("Npgsql:Enum:menu_status", "available,not_active,not_available,sold_out")
                .Annotation("Npgsql:Enum:menu_type", "others,main_course,drinks,dessert")
                .Annotation("Npgsql:Enum:order_status", "placed,preparing,completed,rejected,paid")
                .Annotation("Npgsql:Enum:payment_type", "cash,qr_scan,credit_card")
                .OldAnnotation("Npgsql:Enum:entity_state", "detached,unchanged,deleted,modified,added")
                .OldAnnotation("Npgsql:Enum:menu_type", "others,main_course,drinks,dessert")
                .OldAnnotation("Npgsql:Enum:order_status", "placed,preparing,completed,rejected")
                .OldAnnotation("Npgsql:Enum:payment_type", "cash,qr_scan,credit_card");

            migrationBuilder.AddColumn<MenuStatus>(
                name: "menu_status",
                table: "tbl_menu",
                type: "menu_status",
                nullable: false,
                defaultValue: MenuStatus.Available);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "menu_status",
                table: "tbl_menu");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:entity_state", "detached,unchanged,deleted,modified,added")
                .Annotation("Npgsql:Enum:menu_type", "others,main_course,drinks,dessert")
                .Annotation("Npgsql:Enum:order_status", "placed,preparing,completed,rejected")
                .Annotation("Npgsql:Enum:payment_type", "cash,qr_scan,credit_card")
                .OldAnnotation("Npgsql:Enum:entity_state", "detached,unchanged,deleted,modified,added")
                .OldAnnotation("Npgsql:Enum:menu_status", "available,not_active,not_available,sold_out")
                .OldAnnotation("Npgsql:Enum:menu_type", "others,main_course,drinks,dessert")
                .OldAnnotation("Npgsql:Enum:order_status", "placed,preparing,completed,rejected,paid")
                .OldAnnotation("Npgsql:Enum:payment_type", "cash,qr_scan,credit_card");

            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                table: "tbl_menu",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
