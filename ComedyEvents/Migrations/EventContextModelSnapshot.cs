﻿// <auto-generated />
using System;
using ComedyEvents.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace ComedyEvents.Migrations
{
    [DbContext(typeof(EventContext))]
    partial class EventContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("ComedyEvents.Models.Comedian", b =>
                {
                    b.Property<int>("ComedianId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("ContactPhone")
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .HasColumnType("text");

                    b.HasKey("ComedianId");

                    b.ToTable("Comedians");

                    b.HasData(
                        new
                        {
                            ComedianId = 1,
                            ContactPhone = "111-222-3333",
                            FirstName = "John",
                            LastName = "Rambo"
                        },
                        new
                        {
                            ComedianId = 2,
                            ContactPhone = "444-555-6666",
                            FirstName = "Rocky",
                            LastName = "Balboa"
                        });
                });

            modelBuilder.Entity("ComedyEvents.Models.Event", b =>
                {
                    b.Property<int>("EventId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("EventDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("EventName")
                        .HasColumnType("text");

                    b.Property<int?>("VenueId")
                        .HasColumnType("integer");

                    b.HasKey("EventId");

                    b.HasIndex("VenueId");

                    b.ToTable("Events");

                    b.HasData(
                        new
                        {
                            EventId = 1,
                            EventDate = new DateTime(2020, 5, 17, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EventName = "Funny Comedy Night",
                            VenueId = 1
                        });
                });

            modelBuilder.Entity("ComedyEvents.Models.Gig", b =>
                {
                    b.Property<int>("GigId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int?>("ComedianId")
                        .HasColumnType("integer");

                    b.Property<int?>("EventId")
                        .HasColumnType("integer");

                    b.Property<string>("GigHeadline")
                        .HasColumnType("text");

                    b.Property<int>("GigLengthInMinutes")
                        .HasColumnType("integer");

                    b.HasKey("GigId");

                    b.HasIndex("ComedianId");

                    b.HasIndex("EventId");

                    b.ToTable("Gigs");

                    b.HasData(
                        new
                        {
                            GigId = 1,
                            ComedianId = 1,
                            EventId = 1,
                            GigHeadline = "Funny Show Title",
                            GigLengthInMinutes = 60
                        },
                        new
                        {
                            GigId = 2,
                            ComedianId = 2,
                            EventId = 1,
                            GigHeadline = "Lifetime Of Fun",
                            GigLengthInMinutes = 45
                        });
                });

            modelBuilder.Entity("ComedyEvents.Models.Venue", b =>
                {
                    b.Property<int>("VenueId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("City")
                        .HasColumnType("text");

                    b.Property<int>("Seating")
                        .HasColumnType("integer");

                    b.Property<bool>("ServesAlcohol")
                        .HasColumnType("boolean");

                    b.Property<string>("State")
                        .HasColumnType("text");

                    b.Property<string>("Street")
                        .HasColumnType("text");

                    b.Property<string>("VenueName")
                        .HasColumnType("text");

                    b.Property<string>("ZipCode")
                        .HasColumnType("text");

                    b.HasKey("VenueId");

                    b.ToTable("Venues");

                    b.HasData(
                        new
                        {
                            VenueId = 1,
                            City = "Wilkes Barre",
                            Seating = 125,
                            ServesAlcohol = true,
                            State = "PA",
                            Street = "123 Main Street",
                            VenueName = "Mohegan Sun",
                            ZipCode = "18702"
                        });
                });

            modelBuilder.Entity("ComedyEvents.Models.Event", b =>
                {
                    b.HasOne("ComedyEvents.Models.Venue", "Venue")
                        .WithMany()
                        .HasForeignKey("VenueId");
                });

            modelBuilder.Entity("ComedyEvents.Models.Gig", b =>
                {
                    b.HasOne("ComedyEvents.Models.Comedian", "Comedian")
                        .WithMany()
                        .HasForeignKey("ComedianId");

                    b.HasOne("ComedyEvents.Models.Event", "Event")
                        .WithMany("Gigs")
                        .HasForeignKey("EventId");
                });
#pragma warning restore 612, 618
        }
    }
}