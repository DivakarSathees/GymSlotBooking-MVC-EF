﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using gymI.Models;

#nullable disable

namespace gymI.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230628082307_initial setup")]
    partial class initialsetup
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("gymI.Models.Booking", b =>
                {
                    b.Property<int>("BookingID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BookingID"), 1L, 1);

                    b.Property<int>("SlotID")
                        .HasColumnType("int");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("BookingID");

                    b.HasIndex("SlotID");

                    b.ToTable("Bookings");
                });

            modelBuilder.Entity("gymI.Models.Slot", b =>
                {
                    b.Property<int>("SlotID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SlotID"), 1L, 1);

                    b.Property<int>("Capacity")
                        .HasColumnType("int");

                    b.Property<int>("Duration")
                        .HasColumnType("int");

                    b.Property<DateTime>("Time")
                        .HasColumnType("datetime2");

                    b.HasKey("SlotID");

                    b.ToTable("Slots");
                });

            modelBuilder.Entity("gymI.Models.Booking", b =>
                {
                    b.HasOne("gymI.Models.Slot", "Slot")
                        .WithMany("Bookings")
                        .HasForeignKey("SlotID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Slot");
                });

            modelBuilder.Entity("gymI.Models.Slot", b =>
                {
                    b.Navigation("Bookings");
                });
#pragma warning restore 612, 618
        }
    }
}
