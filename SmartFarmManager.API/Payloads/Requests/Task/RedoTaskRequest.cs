using System.ComponentModel.DataAnnotations;

namespace SmartFarmManager.API.Payloads.Requests.Task
{
    public class RedoTaskRequest
    {
        [Required]
        public Guid TaskId { get; set; }
        [Required]
        public DateTime DueDate { get; set; }
        [Required]
        public int Session { get; set; }    
    }
}
