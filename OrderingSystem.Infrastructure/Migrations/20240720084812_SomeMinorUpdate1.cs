using System;
using Microsoft.EntityFrameworkCore.Migrations;
using OrderingSystem.Infrastructure.Databases.OrderingSystem;

#nullable disable

namespace OrderingSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SomeMinorUpdate1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_order_to_reciept");

            migrationBuilder.DropTable(
                name: "tbl_payment");

            migrationBuilder.AlterColumn<string>(
                name: "number",
                table: "tbl_table",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateTable(
                name: "tbl_receipt",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    timestamp_created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    timestamp_updated = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    timestamp_deleted = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    total = table.Column<double>(type: "double precision", nullable: false),
                    payment_type = table.Column<PaymentType>(type: "payment_type", nullable: false),
                    transaction_id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_receipt", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_order_to_receipt",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    timestamp_created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    timestamp_updated = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    order_id = table.Column<Guid>(type: "uuid", nullable: false),
                    receipt_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_order_to_receipt", x => x.id);
                    table.ForeignKey(
                        name: "fk_tbl_order_to_receipt_tbl_base_soft_delete_receipt_id",
                        column: x => x.receipt_id,
                        principalTable: "tbl_receipt",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_tbl_order_to_receipt_tbl_order_order_id",
                        column: x => x.order_id,
                        principalTable: "tbl_order",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_tbl_order_to_receipt_order_id",
                table: "tbl_order_to_receipt",
                column: "order_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_tbl_order_to_receipt_receipt_id",
                table: "tbl_order_to_receipt",
                column: "receipt_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_receipt_is_deleted",
                table: "tbl_receipt",
                column: "is_deleted");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_order_to_receipt");

            migrationBuilder.DropTable(
                name: "tbl_receipt");

            migrationBuilder.AlterColumn<string>(
                name: "number",
                table: "tbl_table",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10);

            migrationBuilder.CreateTable(
                name: "tbl_payment",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    timestamp_created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    timestamp_deleted = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    timestamp_updated = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    payment_type = table.Column<PaymentType>(type: "payment_type", nullable: false),
                    total = table.Column<double>(type: "double precision", nullable: false),
                    transaction_id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_payment", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_order_to_reciept",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    timestamp_created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    timestamp_updated = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    order_id = table.Column<Guid>(type: "uuid", nullable: false),
                    reciept_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_order_to_reciept", x => x.id);
                    table.ForeignKey(
                        name: "fk_tbl_order_to_reciept_tbl_order_order_id",
                        column: x => x.order_id,
                        principalTable: "tbl_order",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_tbl_order_to_reciept_tbl_payment_reciept_id",
                        column: x => x.reciept_id,
                        principalTable: "tbl_payment",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "ix_tbl_order_to_reciept_order_id",
                table: "tbl_order_to_reciept",
                column: "order_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_tbl_order_to_reciept_reciept_id",
                table: "tbl_order_to_reciept",
                column: "reciept_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_payment_is_deleted",
                table: "tbl_payment",
                column: "is_deleted");
        }
    }
}
