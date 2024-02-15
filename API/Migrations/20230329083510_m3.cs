using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ChatAppApi.Migrations
{
    /// <inheritdoc />
    public partial class m3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TblUsers",
                table: "TblUsers");

            migrationBuilder.RenameTable(
                name: "TblUsers",
                newName: "tblusers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tblusers",
                table: "tblusers",
                column: "id");

            migrationBuilder.CreateTable(
                name: "tblmessages",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    fromuserid = table.Column<int>(type: "integer", nullable: false),
                    touserid = table.Column<int>(type: "integer", nullable: false),
                    content = table.Column<string>(type: "text", nullable: false),
                    senddateday = table.Column<string>(type: "text", nullable: false),
                    senddatetime = table.Column<string>(type: "text", nullable: false),
                    isread = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblmessages", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbluserfriends",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    userid = table.Column<int>(type: "integer", nullable: false),
                    friendid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbluserfriends", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblmessages");

            migrationBuilder.DropTable(
                name: "tbluserfriends");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tblusers",
                table: "tblusers");

            migrationBuilder.RenameTable(
                name: "tblusers",
                newName: "TblUsers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TblUsers",
                table: "TblUsers",
                column: "id");
        }
    }
}
