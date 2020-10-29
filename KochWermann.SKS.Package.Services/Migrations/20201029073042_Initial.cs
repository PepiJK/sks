using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

namespace KochWermann.SKS.Package.Services.Migrations
{
    [ExcludeFromCodeCoverage]
    public partial class Initial : Migration
    {
        /// <summary>
        /// 
        /// </summary>
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Hops",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HopType = table.Column<string>(nullable: false),
                    Code = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    ProcessingDelayMins = table.Column<int>(nullable: true),
                    LocationName = table.Column<string>(nullable: true),
                    LocationCoordinates = table.Column<Point>(type: "Geometry", nullable: true),
                    RegionGeometry = table.Column<Geometry>(type: "Geometry", nullable: true),
                    LogisticsPartner = table.Column<string>(nullable: true),
                    LogisticsPartnerUrl = table.Column<string>(nullable: true),
                    Truck_RegionGeometry = table.Column<Geometry>(type: "Geometry", nullable: true),
                    NumberPlate = table.Column<string>(nullable: true),
                    Level = table.Column<int>(nullable: true),
                    IsRootWarehouse = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hops", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Recipients",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Street = table.Column<string>(nullable: false),
                    PostalCode = table.Column<string>(nullable: false),
                    City = table.Column<string>(nullable: false),
                    Country = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WarehouseNextHops",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TraveltimeMins = table.Column<int>(nullable: true),
                    HopId = table.Column<int>(nullable: false),
                    WarehouseId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarehouseNextHops", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WarehouseNextHops_Hops_HopId",
                        column: x => x.HopId,
                        principalTable: "Hops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WarehouseNextHops_Hops_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Hops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Parcels",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Weight = table.Column<float>(nullable: true),
                    RecipientId = table.Column<int>(nullable: true),
                    SenderId = table.Column<int>(nullable: true),
                    TrackingId = table.Column<string>(nullable: false),
                    State = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parcels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Parcels_Recipients_RecipientId",
                        column: x => x.RecipientId,
                        principalTable: "Recipients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Parcels_Recipients_SenderId",
                        column: x => x.SenderId,
                        principalTable: "Recipients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HopArrivals",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    DateTime = table.Column<DateTime>(nullable: true),
                    ParcelId = table.Column<int>(nullable: true),
                    ParcelId1 = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HopArrivals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HopArrivals_Parcels_ParcelId",
                        column: x => x.ParcelId,
                        principalTable: "Parcels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HopArrivals_Parcels_ParcelId1",
                        column: x => x.ParcelId1,
                        principalTable: "Parcels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HopArrivals_ParcelId",
                table: "HopArrivals",
                column: "ParcelId");

            migrationBuilder.CreateIndex(
                name: "IX_HopArrivals_ParcelId1",
                table: "HopArrivals",
                column: "ParcelId1");

            migrationBuilder.CreateIndex(
                name: "IX_Parcels_RecipientId",
                table: "Parcels",
                column: "RecipientId");

            migrationBuilder.CreateIndex(
                name: "IX_Parcels_SenderId",
                table: "Parcels",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseNextHops_HopId",
                table: "WarehouseNextHops",
                column: "HopId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseNextHops_WarehouseId",
                table: "WarehouseNextHops",
                column: "WarehouseId");
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HopArrivals");

            migrationBuilder.DropTable(
                name: "WarehouseNextHops");

            migrationBuilder.DropTable(
                name: "Parcels");

            migrationBuilder.DropTable(
                name: "Hops");

            migrationBuilder.DropTable(
                name: "Recipients");
        }
    }
}
