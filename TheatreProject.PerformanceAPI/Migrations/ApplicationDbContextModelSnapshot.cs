﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TheatreProject.PerformanceAPI.Data;

#nullable disable

namespace TheatreProject.PerformanceAPI.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TheatreProject.PerformanceAPI.Models.Performance", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("AvailableSeats")
                        .HasColumnType("int");

                    b.Property<decimal>("BasePrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Capacity")
                        .HasColumnType("int");

                    b.Property<int>("Category")
                        .HasColumnType("int");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<TimeSpan>("Duration")
                        .HasColumnType("time");

                    b.Property<string>("ImageLocalPath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Revenue")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("ShowDateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("TheatreName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TotalBookings")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Performances");

                    b.HasData(
                        new
                        {
                            Id = new Guid("d2b8d480-3071-4283-a59f-3dd8ad4ec4ac"),
                            Address = "123 Theatre St",
                            AvailableSeats = 500,
                            BasePrice = 150.00m,
                            Capacity = 500,
                            Category = 1,
                            City = "London",
                            CreatedDate = new DateTime(2025, 1, 20, 6, 49, 54, 324, DateTimeKind.Utc).AddTicks(2226),
                            Description = "Shakespeare's classic tragedy of star-crossed lovers",
                            Duration = new TimeSpan(0, 2, 30, 0, 0),
                            ImageUrl = "https://azurestorage90bh.blob.core.windows.net/theatre/Romeo%20and%20Juliet.jpg",
                            Name = "Romeo and Juliet",
                            Revenue = 0m,
                            ShowDateTime = new DateTime(2025, 1, 27, 8, 49, 54, 324, DateTimeKind.Local).AddTicks(2164),
                            Status = 1,
                            TheatreName = "Royal Theatre",
                            TotalBookings = 0
                        },
                        new
                        {
                            Id = new Guid("716af5d7-6ab1-4221-bafc-8efeaad3515b"),
                            Address = "1 Theatre Square",
                            AvailableSeats = 800,
                            BasePrice = 175.00m,
                            Capacity = 800,
                            Category = 4,
                            City = "Moscow",
                            CreatedDate = new DateTime(2025, 1, 20, 6, 49, 54, 324, DateTimeKind.Utc).AddTicks(2241),
                            Description = "Classic ballet by Tchaikovsky",
                            Duration = new TimeSpan(0, 3, 0, 0, 0),
                            ImageUrl = "https://azurestorage90bh.blob.core.windows.net/theatre/Swan%20Lake.jpg",
                            Name = "Swan Lake",
                            Revenue = 0m,
                            ShowDateTime = new DateTime(2025, 2, 3, 8, 49, 54, 324, DateTimeKind.Local).AddTicks(2236),
                            Status = 1,
                            TheatreName = "Bolshoi Theatre",
                            TotalBookings = 0
                        },
                        new
                        {
                            Id = new Guid("60897e5d-6175-48b0-946c-44e9564f8fb8"),
                            Address = "1681 Broadway",
                            AvailableSeats = 600,
                            BasePrice = 100.00m,
                            Capacity = 600,
                            Category = 2,
                            City = "New York",
                            CreatedDate = new DateTime(2025, 1, 20, 6, 49, 54, 324, DateTimeKind.Utc).AddTicks(2249),
                            Description = "Andrew Lloyd Webber's masterpiece",
                            Duration = new TimeSpan(0, 2, 30, 0, 0),
                            ImageUrl = "https://azurestorage90bh.blob.core.windows.net/theatre/The%20Phantom%20of%20the%20Opera.jpg",
                            Name = "The Phantom of the Opera",
                            Revenue = 0m,
                            ShowDateTime = new DateTime(2025, 1, 25, 8, 49, 54, 324, DateTimeKind.Local).AddTicks(2246),
                            Status = 1,
                            TheatreName = "Broadway Theatre",
                            TotalBookings = 0
                        },
                        new
                        {
                            Id = new Guid("491d15f9-b040-4946-a09c-469da0cf4a7b"),
                            Address = "21 New Globe Walk",
                            AvailableSeats = 400,
                            BasePrice = 140.00m,
                            Capacity = 400,
                            Category = 3,
                            City = "London",
                            CreatedDate = new DateTime(2025, 1, 20, 6, 49, 54, 324, DateTimeKind.Utc).AddTicks(2256),
                            Description = "Shakespeare's shortest and most farcical comedy",
                            Duration = new TimeSpan(0, 2, 0, 0, 0),
                            ImageUrl = "https://azurestorage90bh.blob.core.windows.net/theatre/The%20Comedy%20of%20Errors.jpg",
                            Name = "The Comedy of Errors",
                            Revenue = 0m,
                            ShowDateTime = new DateTime(2025, 1, 30, 8, 49, 54, 324, DateTimeKind.Local).AddTicks(2253),
                            Status = 1,
                            TheatreName = "Globe Theatre",
                            TotalBookings = 0
                        },
                        new
                        {
                            Id = new Guid("f12139d2-e8d4-4b58-9522-41f55439b3ae"),
                            Address = "Via Filodrammatici 2",
                            AvailableSeats = 700,
                            BasePrice = 185.00m,
                            Capacity = 700,
                            Category = 5,
                            City = "Milan",
                            CreatedDate = new DateTime(2025, 1, 20, 6, 49, 54, 324, DateTimeKind.Utc).AddTicks(2264),
                            Description = "Verdi's beloved opera",
                            Duration = new TimeSpan(0, 3, 0, 0, 0),
                            ImageUrl = "https://azurestorage90bh.blob.core.windows.net/theatre/La%20Traviata.jpg",
                            Name = "La Traviata",
                            Revenue = 0m,
                            ShowDateTime = new DateTime(2025, 2, 10, 8, 49, 54, 324, DateTimeKind.Local).AddTicks(2260),
                            Status = 1,
                            TheatreName = "Teatro alla Scala",
                            TotalBookings = 0
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
