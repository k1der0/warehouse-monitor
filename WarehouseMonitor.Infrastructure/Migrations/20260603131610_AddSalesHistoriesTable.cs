using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WarehouseMonitor.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSalesHistoriesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Forecast_Products_ProductId",
                table: "Forecast");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryLevel_Products_ProductId",
                table: "InventoryLevel");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryLevel_Warehouse_WarehouseId",
                table: "InventoryLevel");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesHistory_Products_ProductId",
                table: "SalesHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_StockMovement_Products_ProductId",
                table: "StockMovement");

            migrationBuilder.DropForeignKey(
                name: "FK_StockMovement_Warehouse_WarehouseId",
                table: "StockMovement");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Warehouse",
                table: "Warehouse");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StockMovement",
                table: "StockMovement");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SalesHistory",
                table: "SalesHistory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InventoryLevel",
                table: "InventoryLevel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Forecast",
                table: "Forecast");

            migrationBuilder.RenameTable(
                name: "Warehouse",
                newName: "Warehouses");

            migrationBuilder.RenameTable(
                name: "StockMovement",
                newName: "StockMovements");

            migrationBuilder.RenameTable(
                name: "SalesHistory",
                newName: "SalesHistories");

            migrationBuilder.RenameTable(
                name: "InventoryLevel",
                newName: "InventoryLevels");

            migrationBuilder.RenameTable(
                name: "Forecast",
                newName: "Forecasts");

            migrationBuilder.RenameIndex(
                name: "IX_StockMovement_WarehouseId",
                table: "StockMovements",
                newName: "IX_StockMovements_WarehouseId");

            migrationBuilder.RenameIndex(
                name: "IX_StockMovement_ProductId",
                table: "StockMovements",
                newName: "IX_StockMovements_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_SalesHistory_ProductId",
                table: "SalesHistories",
                newName: "IX_SalesHistories_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_InventoryLevel_WarehouseId",
                table: "InventoryLevels",
                newName: "IX_InventoryLevels_WarehouseId");

            migrationBuilder.RenameIndex(
                name: "IX_InventoryLevel_ProductId",
                table: "InventoryLevels",
                newName: "IX_InventoryLevels_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_Forecast_ProductId",
                table: "Forecasts",
                newName: "IX_Forecasts_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Warehouses",
                table: "Warehouses",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StockMovements",
                table: "StockMovements",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SalesHistories",
                table: "SalesHistories",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InventoryLevels",
                table: "InventoryLevels",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Forecasts",
                table: "Forecasts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Forecasts_Products_ProductId",
                table: "Forecasts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryLevels_Products_ProductId",
                table: "InventoryLevels",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryLevels_Warehouses_WarehouseId",
                table: "InventoryLevels",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SalesHistories_Products_ProductId",
                table: "SalesHistories",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockMovements_Products_ProductId",
                table: "StockMovements",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockMovements_Warehouses_WarehouseId",
                table: "StockMovements",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Forecasts_Products_ProductId",
                table: "Forecasts");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryLevels_Products_ProductId",
                table: "InventoryLevels");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryLevels_Warehouses_WarehouseId",
                table: "InventoryLevels");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesHistories_Products_ProductId",
                table: "SalesHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_StockMovements_Products_ProductId",
                table: "StockMovements");

            migrationBuilder.DropForeignKey(
                name: "FK_StockMovements_Warehouses_WarehouseId",
                table: "StockMovements");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Warehouses",
                table: "Warehouses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StockMovements",
                table: "StockMovements");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SalesHistories",
                table: "SalesHistories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InventoryLevels",
                table: "InventoryLevels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Forecasts",
                table: "Forecasts");

            migrationBuilder.RenameTable(
                name: "Warehouses",
                newName: "Warehouse");

            migrationBuilder.RenameTable(
                name: "StockMovements",
                newName: "StockMovement");

            migrationBuilder.RenameTable(
                name: "SalesHistories",
                newName: "SalesHistory");

            migrationBuilder.RenameTable(
                name: "InventoryLevels",
                newName: "InventoryLevel");

            migrationBuilder.RenameTable(
                name: "Forecasts",
                newName: "Forecast");

            migrationBuilder.RenameIndex(
                name: "IX_StockMovements_WarehouseId",
                table: "StockMovement",
                newName: "IX_StockMovement_WarehouseId");

            migrationBuilder.RenameIndex(
                name: "IX_StockMovements_ProductId",
                table: "StockMovement",
                newName: "IX_StockMovement_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_SalesHistories_ProductId",
                table: "SalesHistory",
                newName: "IX_SalesHistory_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_InventoryLevels_WarehouseId",
                table: "InventoryLevel",
                newName: "IX_InventoryLevel_WarehouseId");

            migrationBuilder.RenameIndex(
                name: "IX_InventoryLevels_ProductId",
                table: "InventoryLevel",
                newName: "IX_InventoryLevel_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_Forecasts_ProductId",
                table: "Forecast",
                newName: "IX_Forecast_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Warehouse",
                table: "Warehouse",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StockMovement",
                table: "StockMovement",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SalesHistory",
                table: "SalesHistory",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InventoryLevel",
                table: "InventoryLevel",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Forecast",
                table: "Forecast",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Forecast_Products_ProductId",
                table: "Forecast",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryLevel_Products_ProductId",
                table: "InventoryLevel",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryLevel_Warehouse_WarehouseId",
                table: "InventoryLevel",
                column: "WarehouseId",
                principalTable: "Warehouse",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SalesHistory_Products_ProductId",
                table: "SalesHistory",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockMovement_Products_ProductId",
                table: "StockMovement",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockMovement_Warehouse_WarehouseId",
                table: "StockMovement",
                column: "WarehouseId",
                principalTable: "Warehouse",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
