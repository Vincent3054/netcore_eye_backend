using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace project.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BeforeAnalysisLog",
                columns: table => new
                {
                    B_Id = table.Column<string>(type: "varchar(10)", nullable: false),
                    RawImage = table.Column<string>(type: "NvarChar(Max)", nullable: true),
                    RawTime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BeforeAnalysisLog", x => x.B_Id);
                });

            migrationBuilder.CreateTable(
                name: "Status",
                columns: table => new
                {
                    S_Id = table.Column<string>(type: "varchar(100)", nullable: false),
                    StatusName = table.Column<string>(type: "nvarChar(36)", nullable: true),
                    Message = table.Column<string>(type: "Nvarchar(200)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Status", x => x.S_Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    M_Id = table.Column<string>(type: "varchar(10)", nullable: false),
                    Email = table.Column<string>(type: "varchar(100)", nullable: true),
                    Passsword = table.Column<string>(type: "varchar(100)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(30)", nullable: true),
                    Role = table.Column<bool>(type: "bit", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.M_Id);
                });

            migrationBuilder.CreateTable(
                name: "AnalysisLog",
                columns: table => new
                {
                    A_Id = table.Column<string>(type: "varchar(10)", nullable: false),
                    M_Id = table.Column<string>(nullable: true),
                    B_Id = table.Column<string>(nullable: true),
                    Image = table.Column<string>(type: "NvarChar(Max)", nullable: true),
                    AnalysisTime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnalysisLog", x => x.A_Id);
                    table.ForeignKey(
                        name: "FK_AnalysisLog_BeforeAnalysisLog_B_Id",
                        column: x => x.B_Id,
                        principalTable: "BeforeAnalysisLog",
                        principalColumn: "B_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AnalysisLog_User_M_Id",
                        column: x => x.M_Id,
                        principalTable: "User",
                        principalColumn: "M_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AnalysisStatus",
                columns: table => new
                {
                    AS_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    A_Id = table.Column<string>(nullable: true),
                    S_Id = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnalysisStatus", x => x.AS_Id);
                    table.ForeignKey(
                        name: "FK_AnalysisStatus_AnalysisLog_A_Id",
                        column: x => x.A_Id,
                        principalTable: "AnalysisLog",
                        principalColumn: "A_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AnalysisStatus_Status_S_Id",
                        column: x => x.S_Id,
                        principalTable: "Status",
                        principalColumn: "S_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "BeforeAnalysisLog",
                columns: new[] { "B_Id", "RawImage", "RawTime" },
                values: new object[] { "1", "https://i.imgur.com/cfeJ9j7.png", new DateTime(2020, 7, 26, 15, 6, 53, 622, DateTimeKind.Local).AddTicks(1213) });

            migrationBuilder.InsertData(
                table: "Status",
                columns: new[] { "S_Id", "Message", "StatusName" },
                values: new object[] { "1", "坐姿過於前傾", "坐姿警示" });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "M_Id", "CreateTime", "Email", "Name", "Passsword", "Role" },
                values: new object[] { "1", new DateTime(2020, 7, 26, 15, 6, 53, 610, DateTimeKind.Local).AddTicks(5737), "ok96305@gmail.com", "陳建成", "12345", true });

            migrationBuilder.InsertData(
                table: "AnalysisLog",
                columns: new[] { "A_Id", "AnalysisTime", "B_Id", "Image", "M_Id" },
                values: new object[] { "1", new DateTime(2020, 7, 26, 15, 6, 53, 619, DateTimeKind.Local).AddTicks(8422), "1", "https://i.imgur.com/PuC21Ma.png", "1" });

            migrationBuilder.InsertData(
                table: "AnalysisStatus",
                columns: new[] { "AS_Id", "A_Id", "S_Id" },
                values: new object[] { 1, "1", "1" });

            migrationBuilder.CreateIndex(
                name: "IX_AnalysisLog_B_Id",
                table: "AnalysisLog",
                column: "B_Id");

            migrationBuilder.CreateIndex(
                name: "IX_AnalysisLog_M_Id",
                table: "AnalysisLog",
                column: "M_Id");

            migrationBuilder.CreateIndex(
                name: "IX_AnalysisStatus_A_Id",
                table: "AnalysisStatus",
                column: "A_Id");

            migrationBuilder.CreateIndex(
                name: "IX_AnalysisStatus_S_Id",
                table: "AnalysisStatus",
                column: "S_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnalysisStatus");

            migrationBuilder.DropTable(
                name: "AnalysisLog");

            migrationBuilder.DropTable(
                name: "Status");

            migrationBuilder.DropTable(
                name: "BeforeAnalysisLog");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
