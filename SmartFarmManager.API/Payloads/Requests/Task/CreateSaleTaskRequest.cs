using SmartFarmManager.Service.BusinessModels.Task;
using System.ComponentModel.DataAnnotations;

namespace SmartFarmManager.API.Payloads.Requests.Task
{
    public class CreateSaleTaskRequest
    {
        [Required]
        public Guid FarmingBatchId { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        [StringLength(500)]
        public string? Notes { get; set; }

        [Required]
        public Guid CreatedByUserId { get; set; }
        public CreateSaleTaskModel MapToModel()
        {
            return new CreateSaleTaskModel
            {
                FarmingBatchId = this.FarmingBatchId,
                DueDate = this.DueDate,
                Notes = this.Notes,
                CreatedByUserId = this.CreatedByUserId
            };
        }
    }
}
