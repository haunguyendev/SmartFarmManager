using System.ComponentModel.DataAnnotations;

namespace SmartFarmManager.API.Payloads.Requests.DeadPoultryLog;

public class ReportErrorRequest
{
    [Required(ErrorMessage = "Vui lòng nhập lý do báo cáo nhầm lẫn.")]
    [StringLength(500, ErrorMessage = "Lý do báo cáo không được vượt quá 500 ký tự.")]
    public string ReportNote { get; set; } = string.Empty;
}