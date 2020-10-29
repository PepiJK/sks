﻿// <auto-generated />
using System;
using KochWermann.SKS.Package.DataAccess.Sql;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NetTopologySuite.Geometries;

namespace KochWermann.SKS.Package.Services.Migrations
{
    /// <summary>
    /// 
    /// </summary>
    [DbContext(typeof(DatabaseContext))]
    [Migration("20201029073042_Initial")]
    partial class Initial
    {
        /// <summary>
        /// 
        /// </summary>
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("KochWermann.SKS.Package.DataAccess.Entities.Hop", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HopType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Point>("LocationCoordinates")
                        .HasColumnType("Geometry");

                    b.Property<string>("LocationName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ProcessingDelayMins")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Hops");

                    b.HasDiscriminator<string>("HopType").HasValue("Hop");
                });

            modelBuilder.Entity("KochWermann.SKS.Package.DataAccess.Entities.HopArrival", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ParcelId")
                        .HasColumnType("int");

                    b.Property<int?>("ParcelId1")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ParcelId");

                    b.HasIndex("ParcelId1");

                    b.ToTable("HopArrivals");
                });

            modelBuilder.Entity("KochWermann.SKS.Package.DataAccess.Entities.Parcel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("RecipientId")
                        .HasColumnType("int");

                    b.Property<int?>("SenderId")
                        .HasColumnType("int");

                    b.Property<int?>("State")
                        .HasColumnType("int");

                    b.Property<string>("TrackingId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float?>("Weight")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.HasIndex("RecipientId");

                    b.HasIndex("SenderId");

                    b.ToTable("Parcels");
                });

            modelBuilder.Entity("KochWermann.SKS.Package.DataAccess.Entities.Recipient", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PostalCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Recipients");
                });

            modelBuilder.Entity("KochWermann.SKS.Package.DataAccess.Entities.WarehouseNextHops", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("HopId")
                        .HasColumnType("int");

                    b.Property<int?>("TraveltimeMins")
                        .HasColumnType("int");

                    b.Property<int?>("WarehouseId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("HopId");

                    b.HasIndex("WarehouseId");

                    b.ToTable("WarehouseNextHops");
                });

            modelBuilder.Entity("KochWermann.SKS.Package.DataAccess.Entities.TransferWarehouse", b =>
                {
                    b.HasBaseType("KochWermann.SKS.Package.DataAccess.Entities.Hop");

                    b.Property<string>("LogisticsPartner")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LogisticsPartnerUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Geometry>("RegionGeometry")
                        .IsRequired()
                        .HasColumnType("Geometry");

                    b.HasDiscriminator().HasValue("TransferWarehouse");
                });

            modelBuilder.Entity("KochWermann.SKS.Package.DataAccess.Entities.Truck", b =>
                {
                    b.HasBaseType("KochWermann.SKS.Package.DataAccess.Entities.Hop");

                    b.Property<string>("NumberPlate")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Geometry>("RegionGeometry")
                        .IsRequired()
                        .HasColumnName("Truck_RegionGeometry")
                        .HasColumnType("Geometry");

                    b.HasDiscriminator().HasValue("Truck");
                });

            modelBuilder.Entity("KochWermann.SKS.Package.DataAccess.Entities.Warehouse", b =>
                {
                    b.HasBaseType("KochWermann.SKS.Package.DataAccess.Entities.Hop");

                    b.Property<bool>("IsRootWarehouse")
                        .HasColumnType("bit");

                    b.Property<int?>("Level")
                        .HasColumnType("int");

                    b.HasDiscriminator().HasValue("Warehouse");
                });

            modelBuilder.Entity("KochWermann.SKS.Package.DataAccess.Entities.HopArrival", b =>
                {
                    b.HasOne("KochWermann.SKS.Package.DataAccess.Entities.Parcel", null)
                        .WithMany("FutureHops")
                        .HasForeignKey("ParcelId");

                    b.HasOne("KochWermann.SKS.Package.DataAccess.Entities.Parcel", null)
                        .WithMany("VisitedHops")
                        .HasForeignKey("ParcelId1");
                });

            modelBuilder.Entity("KochWermann.SKS.Package.DataAccess.Entities.Parcel", b =>
                {
                    b.HasOne("KochWermann.SKS.Package.DataAccess.Entities.Recipient", "Recipient")
                        .WithMany()
                        .HasForeignKey("RecipientId");

                    b.HasOne("KochWermann.SKS.Package.DataAccess.Entities.Recipient", "Sender")
                        .WithMany()
                        .HasForeignKey("SenderId");
                });

            modelBuilder.Entity("KochWermann.SKS.Package.DataAccess.Entities.WarehouseNextHops", b =>
                {
                    b.HasOne("KochWermann.SKS.Package.DataAccess.Entities.Hop", "Hop")
                        .WithMany()
                        .HasForeignKey("HopId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("KochWermann.SKS.Package.DataAccess.Entities.Warehouse", null)
                        .WithMany("NextHops")
                        .HasForeignKey("WarehouseId");
                });
#pragma warning restore 612, 618
        }
    }
}
