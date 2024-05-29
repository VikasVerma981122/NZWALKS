using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NZWALKS.API.Migrations
{
    /// <inheritdoc />
    public partial class Seedingdatafordifficultyandregion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Difficulties",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("46fb5b83-dadb-446a-adc6-f378e7ffa515"), "Hard" },
                    { new Guid("4acaa618-b1f7-4af1-8797-876c9e79714f"), "Easy" },
                    { new Guid("e24ae8ae-fef3-4275-aa8d-1c25b6b17a9a"), "Medium" }
                });

            migrationBuilder.InsertData(
                table: "Regions",
                columns: new[] { "Id", "Code", "Name", "RegionImageUrl" },
                values: new object[,]
                {
                    { new Guid("108dbd32-6e7c-49b4-b5b6-80f398e2bac2"), "AKL", "Auckland", "https://images.pexels.com/photos/4350631/pexels-photo-4350631.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1" },
                    { new Guid("14ceba71-4b51-4777-9b17-46602cf66153"), "WLN", "Wellington", "https://images.pexels.com/photos/13918194/pexels-photo-13918194.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1" },
                    { new Guid("6884f7d7-ad1f-4101-8df3-7a6fa7387d81"), "BOP", "Bay of Plenty", null },
                    { new Guid("906cb139-415a-4bbb-a174-1a1faf9fb1f6"), "NEL", "Nelson", "https://images.pexels.com/photos/5169056/pexels-photo-5169056.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: new Guid("46fb5b83-dadb-446a-adc6-f378e7ffa515"));

            migrationBuilder.DeleteData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: new Guid("4acaa618-b1f7-4af1-8797-876c9e79714f"));

            migrationBuilder.DeleteData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: new Guid("e24ae8ae-fef3-4275-aa8d-1c25b6b17a9a"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("108dbd32-6e7c-49b4-b5b6-80f398e2bac2"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("14ceba71-4b51-4777-9b17-46602cf66153"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("6884f7d7-ad1f-4101-8df3-7a6fa7387d81"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("906cb139-415a-4bbb-a174-1a1faf9fb1f6"));
        }
    }
}
