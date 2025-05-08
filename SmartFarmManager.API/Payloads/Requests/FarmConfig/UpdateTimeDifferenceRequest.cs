using System.ComponentModel.DataAnnotations;

namespace SmartFarmManager.API.Payloads.Requests.FarmConfig
{
    public class UpdateTimeDifferenceRequest
    {

        [Required]
        public DateTime NewTime { get; set; }
    }
}
