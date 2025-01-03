using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TheatreProject.PerformanceAPI.Migrations
{
    /// <inheritdoc />
    public partial class StatusPerf : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Performances",
                keyColumn: "Id",
                keyValue: new Guid("55111aa7-bd3d-47ba-827f-c05c2919fdbe"));

            migrationBuilder.DeleteData(
                table: "Performances",
                keyColumn: "Id",
                keyValue: new Guid("580aae4c-9458-410a-9151-f9c49dccf353"));

            migrationBuilder.DeleteData(
                table: "Performances",
                keyColumn: "Id",
                keyValue: new Guid("7f9771fa-1849-41dd-ad23-35846fad003f"));

            migrationBuilder.DeleteData(
                table: "Performances",
                keyColumn: "Id",
                keyValue: new Guid("8ee4f871-3991-4fe9-865b-84775d279e6f"));

            migrationBuilder.DeleteData(
                table: "Performances",
                keyColumn: "Id",
                keyValue: new Guid("f75267a6-e98f-4cca-8cb9-d53d5963139d"));

            migrationBuilder.InsertData(
                table: "Performances",
                columns: new[] { "Id", "Address", "AvailableSeats", "BasePrice", "Capacity", "Category", "City", "CreatedDate", "Description", "Duration", "ImageUrl", "Name", "Revenue", "ShowDateTime", "Status", "TheatreName", "TotalBookings", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("41bc59f7-26b4-416e-9f88-33e5f704f265"), "21 New Globe Walk", 400, 40.00m, 400, 3, "London", new DateTime(2025, 1, 3, 8, 18, 46, 325, DateTimeKind.Utc).AddTicks(1215), "Shakespeare's shortest and most farcical comedy", new TimeSpan(0, 2, 0, 0, 0), "https://azurestorage90bh.blob.core.windows.net/theatre/The%20Comedy%20of%20Errors.jpg", "The Comedy of Errors", 0m, new DateTime(2025, 1, 13, 10, 18, 46, 325, DateTimeKind.Local).AddTicks(1212), 1, "Globe Theatre", 0, null },
                    { new Guid("4d72afb2-39d0-402b-a60d-34a973f69e67"), "1 Theatre Square", 800, 75.00m, 800, 4, "Moscow", new DateTime(2025, 1, 3, 8, 18, 46, 325, DateTimeKind.Utc).AddTicks(1199), "Classic ballet by Tchaikovsky", new TimeSpan(0, 3, 0, 0, 0), "https://azurestorage90bh.blob.core.windows.net/theatre/Swan%20Lake.jpg", "Swan Lake", 0m, new DateTime(2025, 1, 17, 10, 18, 46, 325, DateTimeKind.Local).AddTicks(1195), 1, "Bolshoi Theatre", 0, null },
                    { new Guid("5bb14b45-7b9e-4c10-9e26-f41e01caef0e"), "1681 Broadway", 600, 100.00m, 600, 2, "New York", new DateTime(2025, 1, 3, 8, 18, 46, 325, DateTimeKind.Utc).AddTicks(1208), "Andrew Lloyd Webber's masterpiece", new TimeSpan(0, 2, 30, 0, 0), "https://azurestorage90bh.blob.core.windows.net/theatre/The%20Phantom%20of%20the%20Opera.jpg", "The Phantom of the Opera", 0m, new DateTime(2025, 1, 8, 10, 18, 46, 325, DateTimeKind.Local).AddTicks(1204), 1, "Broadway Theatre", 0, null },
                    { new Guid("924022ee-1271-4426-93f6-fd0f4ac416b0"), "123 Theatre St", 500, 50.00m, 500, 1, "London", new DateTime(2025, 1, 3, 8, 18, 46, 325, DateTimeKind.Utc).AddTicks(1187), "Shakespeare's classic tragedy of star-crossed lovers", new TimeSpan(0, 2, 30, 0, 0), "https://azurestorage90bh.blob.core.windows.net/theatre/Romeo%20and%20Juliet.jpg", "Romeo and Juliet", 0m, new DateTime(2025, 1, 10, 10, 18, 46, 325, DateTimeKind.Local).AddTicks(1124), 1, "Royal Theatre", 0, null },
                    { new Guid("c06da8a7-cd6e-4d8c-bf91-1a79d6d18d95"), "Via Filodrammatici 2", 700, 85.00m, 700, 5, "Milan", new DateTime(2025, 1, 3, 8, 18, 46, 325, DateTimeKind.Utc).AddTicks(1223), "Verdi's beloved opera", new TimeSpan(0, 3, 0, 0, 0), "https://azurestorage90bh.blob.core.windows.net/theatre/La%20Traviata.jpg", "La Traviata", 0m, new DateTime(2025, 1, 24, 10, 18, 46, 325, DateTimeKind.Local).AddTicks(1219), 1, "Teatro alla Scala", 0, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Performances",
                keyColumn: "Id",
                keyValue: new Guid("41bc59f7-26b4-416e-9f88-33e5f704f265"));

            migrationBuilder.DeleteData(
                table: "Performances",
                keyColumn: "Id",
                keyValue: new Guid("4d72afb2-39d0-402b-a60d-34a973f69e67"));

            migrationBuilder.DeleteData(
                table: "Performances",
                keyColumn: "Id",
                keyValue: new Guid("5bb14b45-7b9e-4c10-9e26-f41e01caef0e"));

            migrationBuilder.DeleteData(
                table: "Performances",
                keyColumn: "Id",
                keyValue: new Guid("924022ee-1271-4426-93f6-fd0f4ac416b0"));

            migrationBuilder.DeleteData(
                table: "Performances",
                keyColumn: "Id",
                keyValue: new Guid("c06da8a7-cd6e-4d8c-bf91-1a79d6d18d95"));

            migrationBuilder.InsertData(
                table: "Performances",
                columns: new[] { "Id", "Address", "AvailableSeats", "BasePrice", "Capacity", "Category", "City", "CreatedDate", "Description", "Duration", "ImageUrl", "Name", "Revenue", "ShowDateTime", "Status", "TheatreName", "TotalBookings", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("55111aa7-bd3d-47ba-827f-c05c2919fdbe"), "21 New Globe Walk", 400, 40.00m, 400, 3, "London", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Shakespeare's shortest and most farcical comedy", new TimeSpan(0, 2, 0, 0, 0), "https://azurestorage90bh.blob.core.windows.net/theatre/The%20Comedy%20of%20Errors.jpg", "The Comedy of Errors", 0m, new DateTime(2025, 1, 13, 7, 4, 18, 570, DateTimeKind.Local).AddTicks(4116), 0, "Globe Theatre", 0, null },
                    { new Guid("580aae4c-9458-410a-9151-f9c49dccf353"), "1681 Broadway", 600, 100.00m, 600, 2, "New York", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Andrew Lloyd Webber's masterpiece", new TimeSpan(0, 2, 30, 0, 0), "https://azurestorage90bh.blob.core.windows.net/theatre/The%20Phantom%20of%20the%20Opera.jpg", "The Phantom of the Opera", 0m, new DateTime(2025, 1, 8, 7, 4, 18, 570, DateTimeKind.Local).AddTicks(4109), 0, "Broadway Theatre", 0, null },
                    { new Guid("7f9771fa-1849-41dd-ad23-35846fad003f"), "Via Filodrammatici 2", 700, 85.00m, 700, 5, "Milan", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Verdi's beloved opera", new TimeSpan(0, 3, 0, 0, 0), "https://azurestorage90bh.blob.core.windows.net/theatre/La%20Traviata.jpg", "La Traviata", 0m, new DateTime(2025, 1, 24, 7, 4, 18, 570, DateTimeKind.Local).AddTicks(4122), 0, "Teatro alla Scala", 0, null },
                    { new Guid("8ee4f871-3991-4fe9-865b-84775d279e6f"), "123 Theatre St", 500, 50.00m, 500, 1, "London", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Shakespeare's classic tragedy of star-crossed lovers", new TimeSpan(0, 2, 30, 0, 0), "https://azurestorage90bh.blob.core.windows.net/theatre/Romeo%20and%20Juliet.jpg", "Romeo and Juliet", 0m, new DateTime(2025, 1, 10, 7, 4, 18, 570, DateTimeKind.Local).AddTicks(4032), 0, "Royal Theatre", 0, null },
                    { new Guid("f75267a6-e98f-4cca-8cb9-d53d5963139d"), "1 Theatre Square", 800, 75.00m, 800, 4, "Moscow", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Classic ballet by Tchaikovsky", new TimeSpan(0, 3, 0, 0, 0), "https://azurestorage90bh.blob.core.windows.net/theatre/Swan%20Lake.jpg", "Swan Lake", 0m, new DateTime(2025, 1, 17, 7, 4, 18, 570, DateTimeKind.Local).AddTicks(4101), 0, "Bolshoi Theatre", 0, null }
                });
        }
    }
}
