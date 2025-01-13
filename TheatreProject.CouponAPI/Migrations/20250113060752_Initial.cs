using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TheatreProject.CouponAPI.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Coupons",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CouponCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiscountAmount = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coupons", x => x.Id);
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Coupons");
        }
    }
}
