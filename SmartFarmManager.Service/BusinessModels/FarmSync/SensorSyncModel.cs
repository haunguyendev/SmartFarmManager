using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFarmManager.Service.BusinessModels.FarmSync
{
    public class SensorSyncModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string SensorCode { get; set; }
        public string PinCode { get; set; }
        public int NodeId { get; set; }
        public string PenId { get; set; }
        public string PenName { get; set; }
        public string SensorTypeId { get; set; }
        public string SensorTypeName { get; set; }
    }
}
