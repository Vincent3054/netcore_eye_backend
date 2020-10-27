using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace project.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Member",
                columns: table => new
                {
                    M_ID = table.Column<string>(type: "varchar(50)", nullable: false),
                    Account = table.Column<string>(type: "varchar(30)", nullable: true),
                    Password = table.Column<string>(type: "varchar(100)", nullable: true),
                    Email = table.Column<string>(type: "varchar(100)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(30)", nullable: true),
                    Sex = table.Column<string>(type: "varchar(2)", nullable: true),
                    BirthDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Role = table.Column<bool>(type: "bit", nullable: false),
                    AuthCode = table.Column<string>(type: "varchar(50)", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Member", x => x.M_ID);
                });

            migrationBuilder.CreateTable(
                name: "Status",
                columns: table => new
                {
                    S_ID = table.Column<string>(type: "varchar(50)", nullable: false),
                    StatusName = table.Column<string>(type: "nvarChar(40)", nullable: true),
                    StatusMessage = table.Column<string>(type: "nvarchar(200)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Status", x => x.S_ID);
                });

            migrationBuilder.CreateTable(
                name: "AnalysisLog",
                columns: table => new
                {
                    A_ID = table.Column<string>(type: "varchar(50)", nullable: false),
                    M_ID = table.Column<string>(nullable: true),
                    Image_Name = table.Column<string>(type: "nvarChar(30)", nullable: true),
                    Image_Location = table.Column<string>(type: "varChar(Max)", nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnalysisLog", x => x.A_ID);
                    table.ForeignKey(
                        name: "FK_AnalysisLog_Member_M_ID",
                        column: x => x.M_ID,
                        principalTable: "Member",
                        principalColumn: "M_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AnalysisStatus",
                columns: table => new
                {
                    AS_ID = table.Column<string>(type: "varchar(50)", nullable: false),
                    A_ID = table.Column<string>(nullable: true),
                    S_ID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnalysisStatus", x => x.AS_ID);
                    table.ForeignKey(
                        name: "FK_AnalysisStatus_AnalysisLog_A_ID",
                        column: x => x.A_ID,
                        principalTable: "AnalysisLog",
                        principalColumn: "A_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AnalysisStatus_Status_S_ID",
                        column: x => x.S_ID,
                        principalTable: "Status",
                        principalColumn: "S_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Member",
                columns: new[] { "M_ID", "Account", "AuthCode", "BirthDate", "CreateTime", "Email", "Name", "Password", "Role", "Sex" },
                values: new object[] { "9dc1dfd0-041c-4f7c-9e9d-efe7afb5ecfd", "admin001", "", new DateTime(2020, 10, 27, 16, 48, 15, 553, DateTimeKind.Local).AddTicks(2470), new DateTime(2020, 10, 27, 16, 48, 15, 555, DateTimeKind.Local).AddTicks(9312), "ok96305@gmail.com", "陳建成", "12345", true, "男" });

            migrationBuilder.InsertData(
                table: "Status",
                columns: new[] { "S_ID", "StatusMessage", "StatusName" },
                values: new object[] { "84d90208-6244-4b83-a714-1baebf96eaa5", "坐姿過於前傾", "坐姿警示" });

            migrationBuilder.InsertData(
                table: "AnalysisLog",
                columns: new[] { "A_ID", "Image_Location", "Image_Name", "M_ID", "UpdateTime" },
                values: new object[] { "9e80196d-072a-4f08-87cd-1c37ccdbc814", "https://i.imgur.com/UL8Jk6A.png", "分析紀錄_2020/10/27 下午 04:48:15", "9dc1dfd0-041c-4f7c-9e9d-efe7afb5ecfd", new DateTime(2020, 10, 27, 16, 48, 15, 565, DateTimeKind.Local).AddTicks(5338) });

            migrationBuilder.InsertData(
                table: "AnalysisStatus",
                columns: new[] { "AS_ID", "A_ID", "S_ID" },
                values: new object[] { "4ccb10d5-8ee9-4110-8423-6be0856a44ed", "9e80196d-072a-4f08-87cd-1c37ccdbc814", "84d90208-6244-4b83-a714-1baebf96eaa5" });

            migrationBuilder.CreateIndex(
                name: "IX_AnalysisLog_M_ID",
                table: "AnalysisLog",
                column: "M_ID");

            migrationBuilder.CreateIndex(
                name: "IX_AnalysisStatus_A_ID",
                table: "AnalysisStatus",
                column: "A_ID");

            migrationBuilder.CreateIndex(
                name: "IX_AnalysisStatus_S_ID",
                table: "AnalysisStatus",
                column: "S_ID");
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
                name: "Member");
        }
    }
}
