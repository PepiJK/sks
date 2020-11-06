using Microsoft.EntityFrameworkCore.Migrations;

namespace KochWermann.SKS.Package.Services.Migrations
{
    /// <summary>
    /// 
    /// </summary>
    public partial class UpdatedHopAndHopArrival : Migration
    {
        /// <summary>
        /// 
        /// </summary>
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HopArrivals_Parcels_ParcelId1",
                table: "HopArrivals");

            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseNextHops_Hops_HopId",
                table: "WarehouseNextHops");

            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseNextHops_Hops_WarehouseId",
                table: "WarehouseNextHops");

            migrationBuilder.DropIndex(
                name: "IX_WarehouseNextHops_HopId",
                table: "WarehouseNextHops");

            migrationBuilder.DropIndex(
                name: "IX_WarehouseNextHops_WarehouseId",
                table: "WarehouseNextHops");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Hops",
                table: "Hops");

            migrationBuilder.DropIndex(
                name: "IX_HopArrivals_ParcelId1",
                table: "HopArrivals");

            migrationBuilder.DropColumn(
                name: "HopId",
                table: "WarehouseNextHops");

            migrationBuilder.DropColumn(
                name: "WarehouseId",
                table: "WarehouseNextHops");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Hops");

            migrationBuilder.DropColumn(
                name: "ParcelId1",
                table: "HopArrivals");

            migrationBuilder.AddColumn<string>(
                name: "HopCode",
                table: "WarehouseNextHops",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WarehouseCode",
                table: "WarehouseNextHops",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Hops",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "HopArrivals",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Hops",
                table: "Hops",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseNextHops_HopCode",
                table: "WarehouseNextHops",
                column: "HopCode");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseNextHops_WarehouseCode",
                table: "WarehouseNextHops",
                column: "WarehouseCode");

            migrationBuilder.CreateIndex(
                name: "IX_HopArrivals_Code",
                table: "HopArrivals",
                column: "Code");

            migrationBuilder.AddForeignKey(
                name: "FK_HopArrivals_Hops_Code",
                table: "HopArrivals",
                column: "Code",
                principalTable: "Hops",
                principalColumn: "Code",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseNextHops_Hops_HopCode",
                table: "WarehouseNextHops",
                column: "HopCode",
                principalTable: "Hops",
                principalColumn: "Code",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseNextHops_Hops_WarehouseCode",
                table: "WarehouseNextHops",
                column: "WarehouseCode",
                principalTable: "Hops",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HopArrivals_Hops_Code",
                table: "HopArrivals");

            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseNextHops_Hops_HopCode",
                table: "WarehouseNextHops");

            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseNextHops_Hops_WarehouseCode",
                table: "WarehouseNextHops");

            migrationBuilder.DropIndex(
                name: "IX_WarehouseNextHops_HopCode",
                table: "WarehouseNextHops");

            migrationBuilder.DropIndex(
                name: "IX_WarehouseNextHops_WarehouseCode",
                table: "WarehouseNextHops");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Hops",
                table: "Hops");

            migrationBuilder.DropIndex(
                name: "IX_HopArrivals_Code",
                table: "HopArrivals");

            migrationBuilder.DropColumn(
                name: "HopCode",
                table: "WarehouseNextHops");

            migrationBuilder.DropColumn(
                name: "WarehouseCode",
                table: "WarehouseNextHops");

            migrationBuilder.AddColumn<int>(
                name: "HopId",
                table: "WarehouseNextHops",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WarehouseId",
                table: "WarehouseNextHops",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Hops",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Hops",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "HopArrivals",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<int>(
                name: "ParcelId1",
                table: "HopArrivals",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Hops",
                table: "Hops",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseNextHops_HopId",
                table: "WarehouseNextHops",
                column: "HopId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseNextHops_WarehouseId",
                table: "WarehouseNextHops",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_HopArrivals_ParcelId1",
                table: "HopArrivals",
                column: "ParcelId1");

            migrationBuilder.AddForeignKey(
                name: "FK_HopArrivals_Parcels_ParcelId1",
                table: "HopArrivals",
                column: "ParcelId1",
                principalTable: "Parcels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseNextHops_Hops_HopId",
                table: "WarehouseNextHops",
                column: "HopId",
                principalTable: "Hops",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseNextHops_Hops_WarehouseId",
                table: "WarehouseNextHops",
                column: "WarehouseId",
                principalTable: "Hops",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
