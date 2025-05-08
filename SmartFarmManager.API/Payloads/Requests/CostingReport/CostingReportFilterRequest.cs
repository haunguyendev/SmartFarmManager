namespace SmartFarmManager.API.Payloads.Requests.CostingReport
{
    public class CostingReportFilterRequest
    {
        public string? KeySearch { get; set; }
        public Guid? FarmId { get; set; }
        public string? CostType { get; set; }
        public int? ReportMonth { get; set; }
        public int? ReportYear { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }

}
