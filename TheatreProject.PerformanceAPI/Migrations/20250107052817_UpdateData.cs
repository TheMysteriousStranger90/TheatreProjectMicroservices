using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TheatreProject.PerformanceAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Performances",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "ImageLocalPath",
                table: "Performances",
                type: "nvarchar(max)",
                nullable: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "ImageLocalPath",
                table: "Performances");

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Performances",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

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
    }
}
