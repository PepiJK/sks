using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KochWermann.SKS.Package.Services.Migrations
{
    [ExcludeFromCodeCoverage]
    public partial class UpdatedHopArrivalRelations : Migration
    {
        /// <summary>
        /// 
        /// </summary>
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HopArrivals_Parcels_ParcelId",
                table: "HopArrivals");

            migrationBuilder.DropIndex(
                name: "IX_HopArrivals_ParcelId",
                table: "HopArrivals");

            migrationBuilder.DropColumn(
                name: "ParcelId",
                table: "HopArrivals");

            migrationBuilder.AddColumn<int>(
                name: "FutureParcelId",
                table: "HopArrivals",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VisitedParcelId",
                table: "HopArrivals",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HopArrivals_FutureParcelId",
                table: "HopArrivals",
                column: "FutureParcelId");

            migrationBuilder.CreateIndex(
                name: "IX_HopArrivals_VisitedParcelId",
                table: "HopArrivals",
                column: "VisitedParcelId");

            migrationBuilder.AddForeignKey(
                name: "FK_HopArrivals_Parcels_FutureParcelId",
                table: "HopArrivals",
                column: "FutureParcelId",
                principalTable: "Parcels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HopArrivals_Parcels_VisitedParcelId",
                table: "HopArrivals",
                column: "VisitedParcelId",
                principalTable: "Parcels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HopArrivals_Parcels_FutureParcelId",
                table: "HopArrivals");

            migrationBuilder.DropForeignKey(
                name: "FK_HopArrivals_Parcels_VisitedParcelId",
                table: "HopArrivals");

            migrationBuilder.DropIndex(
                name: "IX_HopArrivals_FutureParcelId",
                table: "HopArrivals");

            migrationBuilder.DropIndex(
                name: "IX_HopArrivals_VisitedParcelId",
                table: "HopArrivals");

            migrationBuilder.DropColumn(
                name: "FutureParcelId",
                table: "HopArrivals");

            migrationBuilder.DropColumn(
                name: "VisitedParcelId",
                table: "HopArrivals");

            migrationBuilder.AddColumn<int>(
                name: "ParcelId",
                table: "HopArrivals",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HopArrivals_ParcelId",
                table: "HopArrivals",
                column: "ParcelId");

            migrationBuilder.AddForeignKey(
                name: "FK_HopArrivals_Parcels_ParcelId",
                table: "HopArrivals",
                column: "ParcelId",
                principalTable: "Parcels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
