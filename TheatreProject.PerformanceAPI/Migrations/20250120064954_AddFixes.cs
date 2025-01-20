using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TheatreProject.PerformanceAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddFixes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Performances",
                keyColumn: "Id",
                keyValue: new Guid("31dbbf3b-aab3-4716-aaba-9638ac93cf09"));

            migrationBuilder.DeleteData(
                table: "Performances",
                keyColumn: "Id",
                keyValue: new Guid("3fa637e4-7bd2-4886-9647-918d79d9c488"));

            migrationBuilder.DeleteData(
                table: "Performances",
                keyColumn: "Id",
                keyValue: new Guid("4ffc6461-b99f-42c0-bf64-36ca976803c3"));

            migrationBuilder.DeleteData(
                table: "Performances",
                keyColumn: "Id",
                keyValue: new Guid("c3082ec5-ca83-4f70-bd14-47348b2df104"));

            migrationBuilder.DeleteData(
                table: "Performances",
                keyColumn: "Id",
                keyValue: new Guid("d09955b1-6a72-41b6-a079-b64e8ebd8f93"));

            migrationBuilder.InsertData(
                table: "Performances",
                columns: new[] { "Id", "Address", "AvailableSeats", "BasePrice", "Capacity", "Category", "City", "CreatedDate", "Description", "Duration", "ImageLocalPath", "ImageUrl", "Name", "Revenue", "ShowDateTime", "Status", "TheatreName", "TotalBookings", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("491d15f9-b040-4946-a09c-469da0cf4a7b"), "21 New Globe Walk", 400, 140.00m, 400, 3, "London", new DateTime(2025, 1, 20, 6, 49, 54, 324, DateTimeKind.Utc).AddTicks(2256), "Shakespeare's shortest and most farcical comedy", new TimeSpan(0, 2, 0, 0, 0), null, "https://azurestorage90bh.blob.core.windows.net/theatre/The%20Comedy%20of%20Errors.jpg", "The Comedy of Errors", 0m, new DateTime(2025, 1, 30, 8, 49, 54, 324, DateTimeKind.Local).AddTicks(2253), 1, "Globe Theatre", 0, null },
                    { new Guid("60897e5d-6175-48b0-946c-44e9564f8fb8"), "1681 Broadway", 600, 100.00m, 600, 2, "New York", new DateTime(2025, 1, 20, 6, 49, 54, 324, DateTimeKind.Utc).AddTicks(2249), "Andrew Lloyd Webber's masterpiece", new TimeSpan(0, 2, 30, 0, 0), null, "https://azurestorage90bh.blob.core.windows.net/theatre/The%20Phantom%20of%20the%20Opera.jpg", "The Phantom of the Opera", 0m, new DateTime(2025, 1, 25, 8, 49, 54, 324, DateTimeKind.Local).AddTicks(2246), 1, "Broadway Theatre", 0, null },
                    { new Guid("716af5d7-6ab1-4221-bafc-8efeaad3515b"), "1 Theatre Square", 800, 175.00m, 800, 4, "Moscow", new DateTime(2025, 1, 20, 6, 49, 54, 324, DateTimeKind.Utc).AddTicks(2241), "Classic ballet by Tchaikovsky", new TimeSpan(0, 3, 0, 0, 0), null, "https://azurestorage90bh.blob.core.windows.net/theatre/Swan%20Lake.jpg", "Swan Lake", 0m, new DateTime(2025, 2, 3, 8, 49, 54, 324, DateTimeKind.Local).AddTicks(2236), 1, "Bolshoi Theatre", 0, null },
                    { new Guid("d2b8d480-3071-4283-a59f-3dd8ad4ec4ac"), "123 Theatre St", 500, 150.00m, 500, 1, "London", new DateTime(2025, 1, 20, 6, 49, 54, 324, DateTimeKind.Utc).AddTicks(2226), "Shakespeare's classic tragedy of star-crossed lovers", new TimeSpan(0, 2, 30, 0, 0), null, "https://azurestorage90bh.blob.core.windows.net/theatre/Romeo%20and%20Juliet.jpg", "Romeo and Juliet", 0m, new DateTime(2025, 1, 27, 8, 49, 54, 324, DateTimeKind.Local).AddTicks(2164), 1, "Royal Theatre", 0, null },
                    { new Guid("f12139d2-e8d4-4b58-9522-41f55439b3ae"), "Via Filodrammatici 2", 700, 185.00m, 700, 5, "Milan", new DateTime(2025, 1, 20, 6, 49, 54, 324, DateTimeKind.Utc).AddTicks(2264), "Verdi's beloved opera", new TimeSpan(0, 3, 0, 0, 0), null, "https://azurestorage90bh.blob.core.windows.net/theatre/La%20Traviata.jpg", "La Traviata", 0m, new DateTime(2025, 2, 10, 8, 49, 54, 324, DateTimeKind.Local).AddTicks(2260), 1, "Teatro alla Scala", 0, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Performances",
                keyColumn: "Id",
                keyValue: new Guid("491d15f9-b040-4946-a09c-469da0cf4a7b"));

            migrationBuilder.DeleteData(
                table: "Performances",
                keyColumn: "Id",
                keyValue: new Guid("60897e5d-6175-48b0-946c-44e9564f8fb8"));

            migrationBuilder.DeleteData(
                table: "Performances",
                keyColumn: "Id",
                keyValue: new Guid("716af5d7-6ab1-4221-bafc-8efeaad3515b"));

            migrationBuilder.DeleteData(
                table: "Performances",
                keyColumn: "Id",
                keyValue: new Guid("d2b8d480-3071-4283-a59f-3dd8ad4ec4ac"));

            migrationBuilder.DeleteData(
                table: "Performances",
                keyColumn: "Id",
                keyValue: new Guid("f12139d2-e8d4-4b58-9522-41f55439b3ae"));

            migrationBuilder.InsertData(
                table: "Performances",
                columns: new[] { "Id", "Address", "AvailableSeats", "BasePrice", "Capacity", "Category", "City", "CreatedDate", "Description", "Duration", "ImageLocalPath", "ImageUrl", "Name", "Revenue", "ShowDateTime", "Status", "TheatreName", "TotalBookings", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("31dbbf3b-aab3-4716-aaba-9638ac93cf09"), "Via Filodrammatici 2", 700, 85.00m, 700, 5, "Milan", new DateTime(2025, 1, 7, 5, 28, 16, 959, DateTimeKind.Utc).AddTicks(6766), "Verdi's beloved opera", new TimeSpan(0, 3, 0, 0, 0), null, "https://azurestorage90bh.blob.core.windows.net/theatre/La%20Traviata.jpg", "La Traviata", 0m, new DateTime(2025, 1, 28, 7, 28, 16, 959, DateTimeKind.Local).AddTicks(6764), 1, "Teatro alla Scala", 0, null },
                    { new Guid("3fa637e4-7bd2-4886-9647-918d79d9c488"), "21 New Globe Walk", 400, 40.00m, 400, 3, "London", new DateTime(2025, 1, 7, 5, 28, 16, 959, DateTimeKind.Utc).AddTicks(6762), "Shakespeare's shortest and most farcical comedy", new TimeSpan(0, 2, 0, 0, 0), null, "https://azurestorage90bh.blob.core.windows.net/theatre/The%20Comedy%20of%20Errors.jpg", "The Comedy of Errors", 0m, new DateTime(2025, 1, 17, 7, 28, 16, 959, DateTimeKind.Local).AddTicks(6760), 1, "Globe Theatre", 0, null },
                    { new Guid("4ffc6461-b99f-42c0-bf64-36ca976803c3"), "1 Theatre Square", 800, 75.00m, 800, 4, "Moscow", new DateTime(2025, 1, 7, 5, 28, 16, 959, DateTimeKind.Utc).AddTicks(6752), "Classic ballet by Tchaikovsky", new TimeSpan(0, 3, 0, 0, 0), null, "https://azurestorage90bh.blob.core.windows.net/theatre/Swan%20Lake.jpg", "Swan Lake", 0m, new DateTime(2025, 1, 21, 7, 28, 16, 959, DateTimeKind.Local).AddTicks(6749), 1, "Bolshoi Theatre", 0, null },
                    { new Guid("c3082ec5-ca83-4f70-bd14-47348b2df104"), "123 Theatre St", 500, 50.00m, 500, 1, "London", new DateTime(2025, 1, 7, 5, 28, 16, 959, DateTimeKind.Utc).AddTicks(6745), "Shakespeare's classic tragedy of star-crossed lovers", new TimeSpan(0, 2, 30, 0, 0), null, "https://azurestorage90bh.blob.core.windows.net/theatre/Romeo%20and%20Juliet.jpg", "Romeo and Juliet", 0m, new DateTime(2025, 1, 14, 7, 28, 16, 959, DateTimeKind.Local).AddTicks(6701), 1, "Royal Theatre", 0, null },
                    { new Guid("d09955b1-6a72-41b6-a079-b64e8ebd8f93"), "1681 Broadway", 600, 100.00m, 600, 2, "New York", new DateTime(2025, 1, 7, 5, 28, 16, 959, DateTimeKind.Utc).AddTicks(6757), "Andrew Lloyd Webber's masterpiece", new TimeSpan(0, 2, 30, 0, 0), null, "https://azurestorage90bh.blob.core.windows.net/theatre/The%20Phantom%20of%20the%20Opera.jpg", "The Phantom of the Opera", 0m, new DateTime(2025, 1, 12, 7, 28, 16, 959, DateTimeKind.Local).AddTicks(6755), 1, "Broadway Theatre", 0, null }
                });
        }
    }
}
