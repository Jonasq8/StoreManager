using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoreManager.Migrations
{
    /// <inheritdoc />
    public partial class ChainAndStoreAssoiation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ChainID",
                table: "Stores",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Stores_ChainID",
                table: "Stores",
                column: "ChainID");

            migrationBuilder.AddForeignKey(
                name: "FK_Stores_Chains_ChainID",
                table: "Stores",
                column: "ChainID",
                principalTable: "Chains",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stores_Chains_ChainID",
                table: "Stores");

            migrationBuilder.DropIndex(
                name: "IX_Stores_ChainID",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "ChainID",
                table: "Stores");
        }
    }
}
