using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheatreProject.PerformanceAPI.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Performances",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TheatreName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    BasePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ShowDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Duration = table.Column<TimeSpan>(type: "time", nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false),
                    AvailableSeats = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TotalBookings = table.Column<int>(type: "int", nullable: false),
                    Revenue = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Performances", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Performances",
                columns: new[] { "Id", "Address", "AvailableSeats", "BasePrice", "Capacity", "Category", "City", "CreatedDate", "Description", "Duration", "ImageUrl", "Name", "Revenue", "ShowDateTime", "Status", "TheatreName", "TotalBookings", "UpdatedDate" },
                values: new object[] { new Guid("849daeab-67a1-4885-a751-8b60f73a81ce"), "123 Theatre St", 500, 50.00m, 500, 1, "London", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Shakespeare's classic tragedy", new TimeSpan(0, 2, 30, 0, 0), "https://placehold.co/600x400", "Romeo and Juliet", 0m, new DateTime(2025, 1, 9, 19, 7, 35, 816, DateTimeKind.Local).AddTicks(4240), 0, "Royal Theatre", 0, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Performances");
        }
    }
}
