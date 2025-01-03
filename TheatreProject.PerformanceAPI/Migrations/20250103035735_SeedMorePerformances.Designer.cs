﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TheatreProject.PerformanceAPI.Data;

#nullable disable

namespace TheatreProject.PerformanceAPI.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250103035735_SeedMorePerformances")]
    partial class SeedMorePerformances
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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

                    b.Property<string>("ImageUrl")
                        .IsRequired()
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
                            Id = new Guid("fbab8093-0374-4d0a-9cbc-02acae05292b"),
                            Address = "123 Theatre St",
                            AvailableSeats = 500,
                            BasePrice = 50.00m,
                            Capacity = 500,
                            Category = 1,
                            City = "London",
                            CreatedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Shakespeare's classic tragedy of star-crossed lovers",
                            Duration = new TimeSpan(0, 2, 30, 0, 0),
                            ImageUrl = "https://placehold.co/600x400",
                            Name = "Romeo and Juliet",
                            Revenue = 0m,
                            ShowDateTime = new DateTime(2025, 1, 10, 5, 57, 35, 465, DateTimeKind.Local).AddTicks(9737),
                            Status = 0,
                            TheatreName = "Royal Theatre",
                            TotalBookings = 0
                        },
                        new
                        {
                            Id = new Guid("9703000f-6060-4fc1-8b67-84dc86a677da"),
                            Address = "1 Theatre Square",
                            AvailableSeats = 800,
                            BasePrice = 75.00m,
                            Capacity = 800,
                            Category = 4,
                            City = "Moscow",
                            CreatedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Classic ballet by Tchaikovsky",
                            Duration = new TimeSpan(0, 3, 0, 0, 0),
                            ImageUrl = "https://placehold.co/600x400",
                            Name = "Swan Lake",
                            Revenue = 0m,
                            ShowDateTime = new DateTime(2025, 1, 17, 5, 57, 35, 465, DateTimeKind.Local).AddTicks(9802),
                            Status = 0,
                            TheatreName = "Bolshoi Theatre",
                            TotalBookings = 0
                        },
                        new
                        {
                            Id = new Guid("4fdbff60-d4b5-466f-991b-02ea51198955"),
                            Address = "1681 Broadway",
                            AvailableSeats = 600,
                            BasePrice = 100.00m,
                            Capacity = 600,
                            Category = 2,
                            City = "New York",
                            CreatedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Andrew Lloyd Webber's masterpiece",
                            Duration = new TimeSpan(0, 2, 30, 0, 0),
                            ImageUrl = "https://placehold.co/600x400",
                            Name = "The Phantom of the Opera",
                            Revenue = 0m,
                            ShowDateTime = new DateTime(2025, 1, 8, 5, 57, 35, 465, DateTimeKind.Local).AddTicks(9810),
                            Status = 0,
                            TheatreName = "Broadway Theatre",
                            TotalBookings = 0
                        },
                        new
                        {
                            Id = new Guid("a284a9bc-e824-41a9-9b7f-e2ebfd3ef745"),
                            Address = "21 New Globe Walk",
                            AvailableSeats = 400,
                            BasePrice = 40.00m,
                            Capacity = 400,
                            Category = 3,
                            City = "London",
                            CreatedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Shakespeare's shortest and most farcical comedy",
                            Duration = new TimeSpan(0, 2, 0, 0, 0),
                            ImageUrl = "https://placehold.co/600x400",
                            Name = "The Comedy of Errors",
                            Revenue = 0m,
                            ShowDateTime = new DateTime(2025, 1, 13, 5, 57, 35, 465, DateTimeKind.Local).AddTicks(9817),
                            Status = 0,
                            TheatreName = "Globe Theatre",
                            TotalBookings = 0
                        },
                        new
                        {
                            Id = new Guid("9d758aab-d6e0-48e1-8752-92f55aea0c54"),
                            Address = "Via Filodrammatici 2",
                            AvailableSeats = 700,
                            BasePrice = 85.00m,
                            Capacity = 700,
                            Category = 5,
                            City = "Milan",
                            CreatedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Verdi's beloved opera",
                            Duration = new TimeSpan(0, 3, 0, 0, 0),
                            ImageUrl = "https://placehold.co/600x400",
                            Name = "La Traviata",
                            Revenue = 0m,
                            ShowDateTime = new DateTime(2025, 1, 24, 5, 57, 35, 465, DateTimeKind.Local).AddTicks(9823),
                            Status = 0,
                            TheatreName = "Teatro alla Scala",
                            TotalBookings = 0
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
