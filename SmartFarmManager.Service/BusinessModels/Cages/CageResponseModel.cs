using SmartFarmManager.Service.BusinessModels.FarmingBatch;
using SmartFarmManager.Service.BusinessModels.Sensor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFarmManager.Service.BusinessModels.Cages
{
    public class CageResponseModel
    {
        public Guid Id { get; set; }
        public string PenCode { get; set; }
        public Guid FarmId { get; set; }
        public string Name { get; set; }
        public double Area { get; set; }
        public string Location { get; set; }
        public int Capacity { get; set; }
        public string BoardCode { get; set; }
        public bool BoardStatus { get; set; }
        public DateTime EndDateOfFarmingBatch { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CameraUrl { get; set; }
        public Guid StaffId { get; set; }
        public string? StaffName { get; set; }
        public Guid? CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public bool IsSolationCage { get; set; }
        public List<SensorGroupByNodeModel>? Sensors { get; set; } = new List<SensorGroupByNodeModel>();

        public FarmingBatchModel? FarmingBatch { get; set; }
    }
}
