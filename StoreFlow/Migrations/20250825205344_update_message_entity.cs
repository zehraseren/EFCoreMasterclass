using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoreFlow.Migrations
{
    /// <inheritdoc />
    public partial class update_message_entity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SenderImageUrl",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SenderImageUrl",
                table: "Messages");
        }
    }
}
