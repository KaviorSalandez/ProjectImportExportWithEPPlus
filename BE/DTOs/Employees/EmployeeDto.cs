﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DemoImportExport.Consts;
using DemoImportExport.Enums;

namespace DemoImportExport.DTOs.Employees
{
    public class EmployeeDto
    {
        // ID của nhân viên

        public int EmployeeId { get; set; }
        // Tên Nhân Viên 
        public String EmployeeName { get; set; }
        [Required(ErrorMessage = CDKConst.ERRMSG_DepartmentId)]
        // ID của phòng ban
        public int DepartmentId { get; set; }

        // Tên phòng ban 
        public string DepartmentName { get; set; }

        [Required(ErrorMessage = CDKConst.ERRMSG_PositionId)]
        // ID của chức vụ
        public int PositionId { get; set; }

        // Tên chức vụ 
        public string PositionName { get; set;  }

        [Required(ErrorMessage = CDKConst.ERRMSG_EmployeeCode)]

        // Mã nhân viên
        public string EmployeeCode { get; set; }

        // Ngày sinh
        public DateTime? DOB { get; set; }

        // Giới tính
        public CDKEnum.Gender? Gender { get; set; }

        // Số CMT/CCCD
        public string? IDNo { get; set; }

        // Ngày cấp CMT/CCCD
        public DateTime? IssueDate { get; set; }

        // Nơi cấp CMT/CCCD
        public string? IssuedBy { get; set; }
        [MaxLength(200, ErrorMessage = CDKConst.ERRMSG_MaxLength_Address)]
        // Địa chỉ
        public string? Address { get; set; }

        // Điện thoại di động
        public string? MobilePhone { get; set; }

        // Điện thoại cố định
        public string? LandlinePhone { get; set; }

        // Email
        public string? Email { get; set; }

        // Số tài khoản ngân hàng
        public string? BankAccount { get; set; }

        // Tên ngân hàng
        public string? BankName { get; set; }
        //[MaxLength(200, ErrorMessage = CDKConst.ERRMSG_MaxLength_Address)]
        //// Chi nhánh ngân hàng
        public string? Branch { get; set; }
       

        
    }
}
