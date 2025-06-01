using System;
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
    public class EmployeeExcelDto
    {
        // Mã nhân viên
        [Display(Name = "Mã Nhân Viên")]
        [Required(ErrorMessage = CDKConst.ERRMSG_DepartmentId)]
        public string EmployeeCode { get; set; }
        // Tên Nhân Viên 
        [Display(Name = "Tên Nhân Viên")]
        public string EmployeeName { get; set; }
        // Giới tính
        [Display(Name = "Giới tính")]
        public CDKEnum.Gender? Gender { get; set; }
        // Ngày sinh
        [Display(Name = "Ngày Sinh")]
        public DateTime? DOB { get; set; }

        // Tên chức vụ 
        [Display(Name = "Tên vị trí")]
        public string PositionName { get; set; }

        // Tên phòng ban 
        [Display(Name = "Tên phòng ban")]
        public string DepartmentName { get; set; }

        // Số tài khoản ngân hàng
        [Display(Name = "Số CCCD")]
        public string? IDNo { get; set; }
    }
}
