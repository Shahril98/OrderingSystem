using Microsoft.EntityFrameworkCore.Migrations;
using OrderingSystem.Infrastructure.Databases.OrderingSystem;

#nullable disable

namespace OrderingSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTblOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "instruction",
                table: "tbl_order");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:entity_state", "detached,unchanged,deleted,modified,added")
                .Annotation("Npgsql:Enum:menu_type", "others,main_course,drinks,dessert")
                .Annotation("Npgsql:Enum:order_status", "placed,preparing,completed,rejected")
                .Annotation("Npgsql:Enum:payment_type", "cash,qr_scan,credit_card")
                .OldAnnotation("Npgsql:Enum:entity_state", "detached,unchanged,deleted,modified,added")
                .OldAnnotation("Npgsql:Enum:menu_type", "others,main_course,drinks,dessert")
                .OldAnnotation("Npgsql:Enum:payment_type", "cash,qr_scan,credit_card");

            migrationBuilder.AddColumn<string>(
                name: "note",
                table: "tbl_order",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<OrderStatus>(
                name: "status",
                table: "tbl_order",
                type: "order_status",
                nullable: false,
                defaultValue: OrderStatus.Placed);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "note",
                table: "tbl_order");

            migrationBuilder.DropColumn(
                name: "status",
                table: "tbl_order");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:entity_state", "detached,unchanged,deleted,modified,added")
                .Annotation("Npgsql:Enum:menu_type", "others,main_course,drinks,dessert")
                .Annotation("Npgsql:Enum:payment_type", "cash,qr_scan,credit_card")
                .OldAnnotation("Npgsql:Enum:entity_state", "detached,unchanged,deleted,modified,added")
                .OldAnnotation("Npgsql:Enum:menu_type", "others,main_course,drinks,dessert")
                .OldAnnotation("Npgsql:Enum:order_status", "placed,preparing,completed,rejected")
                .OldAnnotation("Npgsql:Enum:payment_type", "cash,qr_scan,credit_card");

            migrationBuilder.AddColumn<string>(
                name: "instruction",
                table: "tbl_order",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }
    }
}
