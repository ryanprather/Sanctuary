﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Sanctuary.Statistics.Repository.Context;

#nullable disable

namespace Sanctuary.Statistics.Repository.Migrations
{
    [DbContext(typeof(StatisticsContext))]
    [Migration("20250131043325_RenameCsvProp")]
    partial class RenameCsvProp
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Sanctuary.Statistics.Repository.Datasets.StatisticalResult", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ChartDataUri")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CsvDataUri")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("StatisticsJobId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("StatisticsJobId");

                    b.ToTable("StatisticalResults", "Statistics");
                });

            modelBuilder.Entity("Sanctuary.Statistics.Repository.Datasets.StatisticsJob", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset?>("Completed")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("Started")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("StatisticsJobDetailsJson")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Jobs", "Statistics");
                });

            modelBuilder.Entity("Sanctuary.Statistics.Repository.Datasets.StatisticalResult", b =>
                {
                    b.HasOne("Sanctuary.Statistics.Repository.Datasets.StatisticsJob", "StatisticsJob")
                        .WithMany()
                        .HasForeignKey("StatisticsJobId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("StatisticsJob");
                });
#pragma warning restore 612, 618
        }
    }
}
