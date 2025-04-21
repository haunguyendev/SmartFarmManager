using System.ComponentModel.DataAnnotations;

namespace SmartFarmManager.API.Payloads.Requests.FarmingBatch
{
    public class UpdateDeadAnimalRequest
    {
        [Required]
        public int DeadAnimal { get; set; } // Số lượng vật nuôi chết cần thêm
        public string? Note { get; set; }
    }
}
