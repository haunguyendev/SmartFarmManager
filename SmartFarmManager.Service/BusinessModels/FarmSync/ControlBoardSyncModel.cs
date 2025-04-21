using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFarmManager.Service.BusinessModels.FarmSync
{
    public class ControlBoardSyncModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ControlBoardCode { get; set; }
        public int PinCode { get; set; }
        public string PenId { get; set; }
        public string PenName { get; set; }
        public string ControlBoardTypeId { get; set; }
        public string ControlBoardTypeName { get; set; }
    }
}
