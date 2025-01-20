using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TheatreProject.CouponAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddedFixes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: new Guid("3d3b160d-e58a-4312-82bc-28f72e8e1684"));

            migrationBuilder.DeleteData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: new Guid("c4808c4d-c3c3-470d-93dd-4c097a8dc28e"));

            migrationBuilder.DeleteData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: new Guid("daf81db8-7c80-415c-89dc-731c3074e06c"));

            migrationBuilder.DeleteData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: new Guid("f0c479d5-58fc-4af3-acbf-46b04fe7ebc2"));

            migrationBuilder.AlterColumn<decimal>(
                name: "DiscountAmount",
                table: "Coupons",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.InsertData(
                table: "Coupons",
                columns: new[] { "Id", "CouponCode", "DiscountAmount" },
                values: new object[,]
                {
                    { new Guid("744d1a06-9109-48f9-89c6-aecc436b87b0"), "GROUP15", 15m },
                    { new Guid("a6b216dc-724f-4da9-8e33-04dc2cd39e41"), "VIP25", 25m },
                    { new Guid("bec4dda0-ed19-4da3-b7b1-2832c23e31a4"), "EARLY20", 20m },
                    { new Guid("d4349836-e348-41b0-ac7d-10c81cd1de0f"), "NEW10", 10m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: new Guid("744d1a06-9109-48f9-89c6-aecc436b87b0"));

            migrationBuilder.DeleteData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: new Guid("a6b216dc-724f-4da9-8e33-04dc2cd39e41"));

            migrationBuilder.DeleteData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: new Guid("bec4dda0-ed19-4da3-b7b1-2832c23e31a4"));

            migrationBuilder.DeleteData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: new Guid("d4349836-e348-41b0-ac7d-10c81cd1de0f"));

            migrationBuilder.AlterColumn<double>(
                name: "DiscountAmount",
                table: "Coupons",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.InsertData(
                table: "Coupons",
                columns: new[] { "Id", "CouponCode", "DiscountAmount" },
                values: new object[,]
                {
                    { new Guid("3d3b160d-e58a-4312-82bc-28f72e8e1684"), "GROUP15", 15.0 },
                    { new Guid("c4808c4d-c3c3-470d-93dd-4c097a8dc28e"), "EARLY20", 20.0 },
                    { new Guid("daf81db8-7c80-415c-89dc-731c3074e06c"), "NEW10", 10.0 },
                    { new Guid("f0c479d5-58fc-4af3-acbf-46b04fe7ebc2"), "VIP25", 25.0 }
                });
        }
    }
}
