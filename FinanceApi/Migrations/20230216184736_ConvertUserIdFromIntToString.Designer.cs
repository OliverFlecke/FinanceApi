﻿// <auto-generated />
using System;
using FinanceApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FinanceApi.Migrations
{
    [DbContext(typeof(FinanceContext))]
    [Migration("20230216184736_ConvertUserIdFromIntToString")]
    partial class ConvertUserIdFromIntToString
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("FinanceApi.Areas.Account.Models.Account", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text")
                        .HasDefaultValue("USD")
                        .HasColumnName("currency");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<int>("SortKey")
                        .HasColumnType("integer")
                        .HasColumnName("sort_key");

                    b.Property<int>("Type")
                        .HasColumnType("integer")
                        .HasColumnName("type");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_account");

                    b.HasIndex("UserId", "Name")
                        .HasDatabaseName("ix_account_user_id_name");

                    b.ToTable("account", (string)null);
                });

            modelBuilder.Entity("FinanceApi.Areas.Account.Models.AccountEntry", b =>
                {
                    b.Property<Guid>("AccountId")
                        .HasColumnType("uuid")
                        .HasColumnName("account_id");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date")
                        .HasColumnName("date");

                    b.Property<double>("Amount")
                        .HasColumnType("double precision")
                        .HasColumnName("amount");

                    b.HasKey("AccountId", "Date")
                        .HasName("pk_account_entry");

                    b.ToTable("account_entry", (string)null);
                });

            modelBuilder.Entity("FinanceApi.Areas.Stocks.Models.StockLot", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<double>("BuyBrokerage")
                        .HasColumnType("double precision")
                        .HasColumnName("buy_brokerage");

                    b.Property<DateTimeOffset>("BuyDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("buy_date");

                    b.Property<double>("BuyPrice")
                        .HasColumnType("double precision")
                        .HasColumnName("buy_price");

                    b.Property<double>("Shares")
                        .HasColumnType("double precision")
                        .HasColumnName("shares");

                    b.Property<double?>("SoldBrokerage")
                        .HasColumnType("double precision")
                        .HasColumnName("sold_brokerage");

                    b.Property<DateTimeOffset?>("SoldDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("sold_date");

                    b.Property<double?>("SoldPrice")
                        .HasColumnType("double precision")
                        .HasColumnName("sold_price");

                    b.Property<string>("Symbol")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("symbol");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_stock_lot");

                    b.HasIndex("UserId", "Symbol")
                        .HasDatabaseName("ix_stock_lot_user_id_symbol");

                    b.ToTable("stock_lot", (string)null);
                });

            modelBuilder.Entity("FinanceApi.Areas.Stocks.Models.TrackedStock", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text")
                        .HasColumnName("user_id");

                    b.Property<string>("Symbol")
                        .HasColumnType("text")
                        .HasColumnName("symbol");

                    b.HasKey("UserId", "Symbol")
                        .HasName("pk_stock");

                    b.ToTable("stock", (string)null);
                });

            modelBuilder.Entity("FinanceApi.Areas.Account.Models.AccountEntry", b =>
                {
                    b.HasOne("FinanceApi.Areas.Account.Models.Account", "Account")
                        .WithMany("Entries")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_account_entry_account_account_id");

                    b.Navigation("Account");
                });

            modelBuilder.Entity("FinanceApi.Areas.Stocks.Models.StockLot", b =>
                {
                    b.HasOne("FinanceApi.Areas.Stocks.Models.TrackedStock", "TrackedSymbol")
                        .WithMany("Lots")
                        .HasForeignKey("UserId", "Symbol")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_stock_lot_stock_tracked_symbol_temp_id");

                    b.Navigation("TrackedSymbol");
                });

            modelBuilder.Entity("FinanceApi.Areas.Account.Models.Account", b =>
                {
                    b.Navigation("Entries");
                });

            modelBuilder.Entity("FinanceApi.Areas.Stocks.Models.TrackedStock", b =>
                {
                    b.Navigation("Lots");
                });
#pragma warning restore 612, 618
        }
    }
}
