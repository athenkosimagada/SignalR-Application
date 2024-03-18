using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SignalR.Server.Migrations
{
    /// <inheritdoc />
    public partial class UpdateConnectionColumnTableName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "signalRId",
                table: "Connections",
                newName: "signalrId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "signalrId",
                table: "Connections",
                newName: "signalRId");
        }
    }
}
