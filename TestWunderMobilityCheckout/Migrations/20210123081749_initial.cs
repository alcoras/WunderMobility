using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TestWunderMobilityCheckout.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomersList",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    PromotionalSum = table.Column<float>(nullable: false),
                    PromotionalDiscount = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomersList", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EventDataModel",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Outgoing = table.Column<bool>(nullable: false),
                    InputId = table.Column<long>(nullable: false),
                    ParentId = table.Column<long>(nullable: true),
                    EventBody = table.Column<string>(nullable: true),
                    LastSentDateTime = table.Column<DateTime>(nullable: false),
                    Completed = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventDataModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductList",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ProductCode = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Price = table.Column<float>(nullable: false),
                    PromotionalQuantity = table.Column<long>(nullable: false),
                    PromotionalPrice = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductList", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventDataModel_InputId",
                table: "EventDataModel",
                column: "InputId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomersList");

            migrationBuilder.DropTable(
                name: "EventDataModel");

            migrationBuilder.DropTable(
                name: "ProductList");
        }
    }
}
