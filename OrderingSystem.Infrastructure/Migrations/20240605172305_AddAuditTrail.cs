using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderingSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAuditTrail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:entity_state", "detached,unchanged,deleted,modified,added")
                .Annotation("Npgsql:Enum:menu_type", "others,main_course,drinks,dessert")
                .Annotation("Npgsql:Enum:payment_type", "cash,qr_scan,credit_card")
                .OldAnnotation("Npgsql:Enum:menu_type", "others,main_course,drinks,dessert")
                .OldAnnotation("Npgsql:Enum:payment_type", "cash,qr_scan,credit_card");

            migrationBuilder.CreateTable(
                name: "tbl_audit_trail",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    actor_id = table.Column<Guid>(type: "uuid", nullable: false),
                    action = table.Column<EntityState>(type: "entity_state", nullable: false),
                    table_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    table_id = table.Column<Guid>(type: "uuid", nullable: false),
                    action_timestamp = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    data = table.Column<string>(type: "text", maxLength: 2147483647, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tbl_audit_trail", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_audit_trail");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:menu_type", "others,main_course,drinks,dessert")
                .Annotation("Npgsql:Enum:payment_type", "cash,qr_scan,credit_card")
                .OldAnnotation("Npgsql:Enum:entity_state", "detached,unchanged,deleted,modified,added")
                .OldAnnotation("Npgsql:Enum:menu_type", "others,main_course,drinks,dessert")
                .OldAnnotation("Npgsql:Enum:payment_type", "cash,qr_scan,credit_card");
        }
    }
}
