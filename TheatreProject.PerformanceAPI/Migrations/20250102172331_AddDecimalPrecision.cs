using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheatreProject.PerformanceAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddDecimalPrecision : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Performances",
                keyColumn: "Id",
                keyValue: new Guid("849daeab-67a1-4885-a751-8b60f73a81ce"));

            migrationBuilder.InsertData(
                table: "Performances",
                columns: new[] { "Id", "Address", "AvailableSeats", "BasePrice", "Capacity", "Category", "City", "CreatedDate", "Description", "Duration", "ImageUrl", "Name", "Revenue", "ShowDateTime", "Status", "TheatreName", "TotalBookings", "UpdatedDate" },
                values: new object[] { new Guid("7187e907-7dfe-4011-a8e7-0a65b7ac6fb7"), "123 Theatre St", 500, 50.00m, 500, 1, "London", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Shakespeare's classic tragedy", new TimeSpan(0, 2, 30, 0, 0), "https://placehold.co/600x400", "Romeo and Juliet", 0m, new DateTime(2025, 1, 9, 19, 23, 31, 259, DateTimeKind.Local).AddTicks(2345), 0, "Royal Theatre", 0, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Performances",
                keyColumn: "Id",
                keyValue: new Guid("7187e907-7dfe-4011-a8e7-0a65b7ac6fb7"));

            migrationBuilder.InsertData(
                table: "Performances",
                columns: new[] { "Id", "Address", "AvailableSeats", "BasePrice", "Capacity", "Category", "City", "CreatedDate", "Description", "Duration", "ImageUrl", "Name", "Revenue", "ShowDateTime", "Status", "TheatreName", "TotalBookings", "UpdatedDate" },
                values: new object[] { new Guid("849daeab-67a1-4885-a751-8b60f73a81ce"), "123 Theatre St", 500, 50.00m, 500, 1, "London", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Shakespeare's classic tragedy", new TimeSpan(0, 2, 30, 0, 0), "https://placehold.co/600x400", "Romeo and Juliet", 0m, new DateTime(2025, 1, 9, 19, 7, 35, 816, DateTimeKind.Local).AddTicks(4240), 0, "Royal Theatre", 0, null });
        }
    }
}
