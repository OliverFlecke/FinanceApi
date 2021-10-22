using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceApi.Migrations
{
    public partial class StocksAndAccounts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "account",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_account", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "stock",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    symbol = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_stock", x => new { x.user_id, x.symbol });
                });

            migrationBuilder.CreateTable(
                name: "account_entry",
                columns: table => new
                {
                    date = table.Column<DateOnly>(type: "date", nullable: false),
                    account_id = table.Column<Guid>(type: "uuid", nullable: false),
                    amount = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_account_entry", x => new { x.account_id, x.date });
                    table.ForeignKey(
                        name: "fk_account_entry_account_account_id",
                        column: x => x.account_id,
                        principalTable: "account",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "stock_lot",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    symbol = table.Column<string>(type: "text", nullable: false),
                    shares = table.Column<double>(type: "double precision", nullable: false),
                    buy_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    buy_price = table.Column<double>(type: "double precision", nullable: false),
                    buy_brokerage = table.Column<double>(type: "double precision", nullable: false),
                    sold_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    sold_price = table.Column<double>(type: "double precision", nullable: true),
                    sold_brokerage = table.Column<double>(type: "double precision", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_stock_lot", x => x.id);
                    table.ForeignKey(
                        name: "fk_stock_lot_stock_tracked_symbol_temp_id",
                        columns: x => new { x.user_id, x.symbol },
                        principalTable: "stock",
                        principalColumns: new[] { "user_id", "symbol" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_account_user_id_name",
                table: "account",
                columns: new[] { "user_id", "name" });

            migrationBuilder.CreateIndex(
                name: "ix_stock_lot_user_id_symbol",
                table: "stock_lot",
                columns: new[] { "user_id", "symbol" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "account_entry");

            migrationBuilder.DropTable(
                name: "stock_lot");

            migrationBuilder.DropTable(
                name: "account");

            migrationBuilder.DropTable(
                name: "stock");
        }
    }
}
