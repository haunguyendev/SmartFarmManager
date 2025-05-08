namespace SmartFarmManager.API.Payloads.Requests.CostingReport
{
    public class CostingReportGroupFilterRequest
    {
        public int? Year { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
