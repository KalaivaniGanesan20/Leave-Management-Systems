using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace project.Migrations
{
    public partial class Leavemigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "balance",
                columns: table => new
                {
                    Balanceid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Employeeid = table.Column<int>(type: "int", nullable: false),
                    username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    sickleave = table.Column<int>(type: "int", nullable: false),
                    Compensatoryleave = table.Column<int>(type: "int", nullable: false),
                    casualleave = table.Column<int>(type: "int", nullable: false),
                    Lossofpay = table.Column<int>(type: "int", nullable: false),
                    annualleave = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_balance", x => x.Balanceid);
                });

            migrationBuilder.CreateTable(
                name: "leave",
                columns: table => new
                {
                    leaveid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    leavetype = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    userRole = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    leaveReason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    leaveFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    leaveTo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Numberofdays = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_leave", x => x.leaveid);
                });

            migrationBuilder.CreateTable(
                name: "Myleavestatus",
                columns: table => new
                {
                    statusid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ParameterId = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Myleavestatus", x => x.statusid);
                });

            migrationBuilder.CreateTable(
                name: "timesheet",
                columns: table => new
                {
                    TimesheetId = table.Column<int>(type: "int", nullable: false),
                    username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    projectname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    date = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Totalhoursworked = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_timesheet", x => x.TimesheetId);
                });

            migrationBuilder.CreateTable(
                name: "mytask",
                columns: table => new
                {
                    taskid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    taskname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    start_time = table.Column<TimeSpan>(type: "time", nullable: false),
                    end_time = table.Column<TimeSpan>(type: "time", nullable: false),
                    TimesheetId = table.Column<int>(type: "int", nullable: false),
                    hoursworked = table.Column<int>(type: "int", nullable: false),
                    taskstatus = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mytask", x => x.taskid);
                    table.ForeignKey(
                        name: "FK_mytask_timesheet_TimesheetId",
                        column: x => x.TimesheetId,
                        principalTable: "timesheet",
                        principalColumn: "TimesheetId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_mytask_TimesheetId",
                table: "mytask",
                column: "TimesheetId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "balance");

            migrationBuilder.DropTable(
                name: "leave");

            migrationBuilder.DropTable(
                name: "Myleavestatus");

            migrationBuilder.DropTable(
                name: "mytask");

            migrationBuilder.DropTable(
                name: "timesheet");
        }
    }
}
