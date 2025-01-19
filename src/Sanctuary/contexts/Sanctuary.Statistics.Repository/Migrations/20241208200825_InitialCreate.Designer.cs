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
    [Migration("20241208200825_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Sanctuary.Statistics.Repository.Datasets.StatisticalAnalysis", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("GraphData")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("StatisticsJobId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("StatisticsJobId");

                    b.ToTable("StatisticalAnalyses", "Statistics");
                });

            modelBuilder.Entity("Sanctuary.Statistics.Repository.Datasets.StatisticsJob", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("Completed")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset>("Started")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid>("StatisticsJobTypeId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("StatisticsJobTypeId");

                    b.ToTable("StatisticsJobs", "Statistics");
                });

            modelBuilder.Entity("Sanctuary.Statistics.Repository.Datasets.StatisticsJobType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("StatisticsJobTypes", "Statistics");
                });

            modelBuilder.Entity("Sanctuary.Statistics.Repository.Datasets.StatisticalAnalysis", b =>
                {
                    b.HasOne("Sanctuary.Statistics.Repository.Datasets.StatisticsJob", "StatisticsJob")
                        .WithMany()
                        .HasForeignKey("StatisticsJobId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("StatisticsJob");
                });

            modelBuilder.Entity("Sanctuary.Statistics.Repository.Datasets.StatisticsJob", b =>
                {
                    b.HasOne("Sanctuary.Statistics.Repository.Datasets.StatisticsJobType", "StatisticsJobType")
                        .WithMany()
                        .HasForeignKey("StatisticsJobTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("StatisticsJobType");
                });
#pragma warning restore 612, 618
        }
    }
}
