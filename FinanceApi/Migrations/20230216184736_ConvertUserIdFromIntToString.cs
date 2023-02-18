using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceApi.Migrations
{
    /// <inheritdoc />
    public partial class ConvertUserIdFromIntToString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_stock_lot_stock_tracked_symbol_temp_id",
                table: "stock_lot");
            migrationBuilder.AlterColumn<string>(
                name: "user_id",
                table: "stock_lot",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "user_id",
                table: "stock",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "user_id",
                table: "account",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");


            migrationBuilder.Sql("UPDATE stock_lot SET user_id = 'github|' || user_id;");
            migrationBuilder.Sql("UPDATE stock SET user_id = 'github|' || user_id;");
            migrationBuilder.Sql("UPDATE account SET user_id = 'github|' || user_id;");

            migrationBuilder.AddForeignKey(
                name: "fk_stock_lot_stock_tracked_symbol_temp_id",
                table: "stock_lot",
                columns: new[] { "user_id", "symbol" },
                principalTable: "stock",
                principalColumns: new[] { "user_id", "symbol" },
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_stock_lot_stock_tracked_symbol_temp_id",
                table: "stock_lot");

            migrationBuilder.Sql("UPDATE stock_lot SET user_id = replace(user_id, 'github|', '');");
            migrationBuilder.Sql("UPDATE stock SET user_id = replace(user_id, 'github|', '');");
            migrationBuilder.Sql("UPDATE account SET user_id = replace(user_id, 'github|', '');");

            migrationBuilder.Sql("ALTER TABLE stock_lot " +
                "ALTER COLUMN user_id TYPE integer USING user_id::integer");
            migrationBuilder.Sql("ALTER TABLE stock " +
                "ALTER COLUMN user_id TYPE integer USING user_id::integer");
            migrationBuilder.Sql("ALTER TABLE account " +
                "ALTER COLUMN user_id TYPE integer USING user_id::integer");

            migrationBuilder.AddForeignKey(
                name: "fk_stock_lot_stock_tracked_symbol_temp_id",
                table: "stock_lot",
                columns: new[] { "user_id", "symbol" },
                principalTable: "stock",
                principalColumns: new[] { "user_id", "symbol" },
                onDelete: ReferentialAction.Cascade);
        }
    }
}
