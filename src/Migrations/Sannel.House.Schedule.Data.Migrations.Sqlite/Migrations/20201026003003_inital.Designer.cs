﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Sannel.House.Schedule.Data;

namespace Sannel.House.Schedule.Data.Migrations.Sqlite.Migrations
{
    [DbContext(typeof(ScheduleDbContext))]
    [Migration("20201026003003_inital")]
    partial class inital
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.0-rc.2.20475.6");

            modelBuilder.Entity("Sannel.House.Schedule.Models.Schedule", b =>
                {
                    b.Property<long>("ScheduleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double?>("DefaultMaxValue")
                        .HasColumnType("REAL");

                    b.Property<double>("DefaultMinValue")
                        .HasColumnType("REAL");

                    b.Property<double>("MinimumDifference")
                        .HasColumnType("REAL");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ScheduleKey")
                        .HasColumnType("TEXT");

                    b.HasKey("ScheduleId");

                    b.HasIndex("ScheduleKey")
                        .IsUnique();

                    b.ToTable("Schedules");
                });

            modelBuilder.Entity("Sannel.House.Schedule.Models.ScheduleProperty", b =>
                {
                    b.Property<Guid>("SchedulePropertyId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("TEXT");

                    b.Property<long>("ScheduleId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(1024)
                        .HasColumnType("TEXT");

                    b.HasKey("SchedulePropertyId");

                    b.HasIndex("Name");

                    b.HasIndex("ScheduleId", "Name")
                        .IsUnique();

                    b.ToTable("ScheduleProperties");
                });

            modelBuilder.Entity("Sannel.House.Schedule.Models.ScheduleStart", b =>
                {
                    b.Property<Guid>("ScheduleStartId")
                        .HasColumnType("TEXT");

                    b.Property<int?>("DurationMinutes")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<long>("ScheduleId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset>("Start")
                        .HasColumnType("TEXT");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.Property<double?>("ValueMax")
                        .HasColumnType("REAL");

                    b.Property<double>("ValueMin")
                        .HasColumnType("REAL");

                    b.HasKey("ScheduleStartId");

                    b.HasIndex("ScheduleId");

                    b.HasIndex("Start", "Type");

                    b.ToTable("ScheduleStarts");
                });

            modelBuilder.Entity("Sannel.House.Schedule.Models.ScheduleProperty", b =>
                {
                    b.HasOne("Sannel.House.Schedule.Models.Schedule", "Schedule")
                        .WithMany()
                        .HasForeignKey("ScheduleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Schedule");
                });

            modelBuilder.Entity("Sannel.House.Schedule.Models.ScheduleStart", b =>
                {
                    b.HasOne("Sannel.House.Schedule.Models.Schedule", "Schedule")
                        .WithMany()
                        .HasForeignKey("ScheduleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Schedule");
                });
#pragma warning restore 612, 618
        }
    }
}
