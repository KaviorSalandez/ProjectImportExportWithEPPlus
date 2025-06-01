using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DemoImportExport.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "DepartmentId", "DepartmentName" },
                values: new object[,]
                {
                    { 1, "Phòng Kỹ thuật phần mềm" },
                    { 2, "Phòng Nhân sự" },
                    { 3, "Phòng Kế toán" }
                });

            migrationBuilder.InsertData(
                table: "Positions",
                columns: new[] { "PositionId", "PositionName" },
                values: new object[,]
                {
                    { 1, "Lập trình viên" },
                    { 2, "Trưởng phòng" },
                    { 3, "Kế toán viên" }
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "EmployeeId", "Address", "BankAccount", "BankName", "Branch", "DOB", "DepartmentId", "Email", "EmployeeCode", "EmployeeName", "Gender", "IDNo", "IssueDate", "IssuedBy", "LandlinePhone", "MobilePhone", "Password", "PositionId", "TotalRecord" },
                values: new object[,]
                {
                    { 1, "Hà Nội", "123456789", "Vietcombank", "Hà Nội", new DateTime(1990, 5, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "nva@example.com", "NV001", "Nguyễn Văn A", 0, "123456789", new DateTime(2010, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Công an Hà Nội", "0241234567", "0912345678", "123@abc", 1, 10 },
                    { 2, "TP HCM", "987654321", "ACB", "TP HCM", new DateTime(1985, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "ttb@example.com", "NV002", "Trần Thị B", 1, "987654321", new DateTime(2005, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Công an TP HCM", "0281234567", "0909123456", "456@def", 2, 12 },
                    { 3, "Đà Nẵng", "111222333", "BIDV", "Đà Nẵng", new DateTime(1992, 11, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "lvc@example.com", "NV003", "Lê Văn C", 0, "111222333", new DateTime(2012, 9, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Công an Đà Nẵng", "0236123456", "0938123456", "789@ghi", 3, 8 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Positions",
                keyColumn: "PositionId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Positions",
                keyColumn: "PositionId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Positions",
                keyColumn: "PositionId",
                keyValue: 3);
        }
    }
}
