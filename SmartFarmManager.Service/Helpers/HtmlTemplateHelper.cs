using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFarmManager.Service.Helpers
{
    public static class HtmlTemplateHelper
    {
        private const string LogoUrl = "https://imageservice.fjourney.site/images/7cadf1e1-2cd5-45de-b5cc-828a6661eb8c.png";

        private static string GenerateEmailLayout(string title, string bodyContent)
        {
            return $@"
        <html>
        <body style='font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 30px;'>
            <table style='max-width: 600px; width: 100%; margin: auto; background: #ffffff; padding: 30px; border-radius: 8px; box-shadow: 0 2px 8px rgba(0,0,0,0.1);'>
                <tr>
                    <td style='text-align: center;'>
                        <img src='{LogoUrl}' alt='Smart Farm Logo' style='height: 50px; margin-bottom: 20px;' />
                        <h2 style='color: #2c3e50; margin-bottom: 20px;'>{title}</h2>
                    </td>
                </tr>
                <tr>
                    <td style='color: #333333; font-size: 16px;'>
                        {bodyContent}
                    </td>
                </tr>
                <tr>
                    <td style='text-align: center; margin-top: 30px; color: #999999; font-size: 12px;'>
                        <p>Smart Farm Manager &copy; 2025</p>
                    </td>
                </tr>
            </table>
        </body>
        </html>";
        }

        public static string GenerateOverdueTaskEmailForAdmin(string recipientName, string taskName, string cageName, string staffName, DateTime dueDate, string session, TimeSpan timeEnd)
        {
            var bodyContent = $@"
            <p>Xin chào <strong>{recipientName}</strong>,</p>
            <p>Nhiệm vụ <strong>{taskName}</strong> tại chuồng <strong>{cageName}</strong>, do nhân viên <strong>{staffName}</strong> phụ trách, đã quá hạn vào buổi <strong>{session} ({timeEnd})</strong> ngày <strong>{dueDate:yyyy-MM-dd}</strong>.</p>
            <p>Vui lòng kiểm tra và hỗ trợ xử lý nhanh chóng.</p>";
            return GenerateEmailLayout("Thông báo nhiệm vụ quá hạn", bodyContent);
        }

        public static string GenerateOverdueTaskEmailForVet(string cageName, string assignedUserName, string taskName, string diagnosis, DateTime dueDate,string session, TimeSpan timeEnd)
        {
            var bodyContent = $@"
            <p>Xin chào Bác sĩ thú y,</p>
            <p>Công việc <strong>{taskName}</strong> tại chuồng <strong>{cageName}</strong>, do nhân viên <strong>{assignedUserName}</strong> phụ trách, đã quá hạn vào <strong>{session} ({timeEnd})</strong> ngày <strong>{dueDate:yyyy-MM-dd}</strong>.</p>
            <p>Đơn thuốc đang điều trị: <strong>{diagnosis}</strong>.</p>
            <p>Vui lòng kiểm tra và xử lý ngay để đảm bảo sức khỏe vật nuôi.</p>";
            return GenerateEmailLayout("Thông báo công việc thú y quá hạn", bodyContent);
        }

        public static string GenerateOverdueTaskEmailForStaff(string cageName, string taskName, DateTime dueDate, string session, TimeSpan timeEnd)
        {
            var bodyContent = $@"
            <p>Xin chào,</p>
            <p>Công việc <strong>{taskName}</strong> của bạn tại chuồng <strong>{cageName}</strong> đã quá hạn vào <strong>{session} ({timeEnd})</strong> ngày <strong>{dueDate:yyyy-MM-dd}</strong>.</p>
            <p>Vui lòng nhanh chóng hoàn thành hoặc báo cáo lý do chậm trễ.</p>";
            return GenerateEmailLayout("Thông báo công việc quá hạn", bodyContent);
        }
    }


}
