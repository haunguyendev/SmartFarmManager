using System.ComponentModel.DataAnnotations;

namespace SmartFarmManager.API.Payloads.Requests.MasterData
{
    public class MasterDataFilterRequest
    {
        [StringLength(100, ErrorMessage = "KeySearch cannot be longer than 100 characters.")]
        public string? KeySearch { get; set; }

        public string? CostType { get; set; }

        public Guid? FarmId { get; set; }

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 20;
    }
}
