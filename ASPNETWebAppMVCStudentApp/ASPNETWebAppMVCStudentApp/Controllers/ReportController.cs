using ASPNETWebAppMVCStudentApp.Models;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace ASPNETWebAppMVCStudentApp.Controllers
{
    public class ReportController : Controller
    {
        private SchoolDBLab06Entities _context = new SchoolDBLab06Entities();

        // 1. Hiển thị form chọn khóa học
        public ActionResult Index()
        {
            var model = new CourseViewModel
            {
                Courses = new SelectList(_context.Courses, "CourseID", "Title")
            };
            return View(model);
        }

        // 2. Xử lý khi bấm nút xuất PDF
        [HttpPost]
        public ActionResult Generate(int SelectedCourseID, string action)
        {
            // Lấy đường dẫn tới file mẫu RDLC
            var reportPath = Path.Combine(HostingEnvironment.MapPath("~/Reports"), "StudentReport.rdlc");
            var localReport = new LocalReport { ReportPath = reportPath };

            // Truy vấn dữ liệu: Lấy danh sách sinh viên thuộc khóa học được chọn
            var students = _context.Enrollments
                .Where(e => e.CourseID == SelectedCourseID)
                .Select(e => new
                {
                    StudentID = e.Student.StudentID,
                    FirstName = e.Student.FirstName,
                    LastName = e.Student.LastName,
                    Grade = e.Grade
                }).ToList();

            // Đổ dữ liệu vào báo cáo
            localReport.DataSources.Add(new ReportDataSource("StudentDataSet", students));

            // Xuất ra file PDF
            var reportBytes = localReport.Render("PDF");
            return File(reportBytes, "application/pdf", "StudentReport.pdf");
        }
    }
}