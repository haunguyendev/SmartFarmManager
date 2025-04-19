using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFarmManager.Service.BusinessModels.FarmSync
{
    public class CageSyncModel
    {
        public string Id { get; set; }
        public string PenCode { get; set; }
        public string Name { get; set; }
        public int TopicCode { get; set; }
        public string CameraUrl { get; set; }
        public int ChannelId { get; set; }
        public int Nodes { get; set; }
        public string FarmId { get; set; }

        public List<SensorSyncModel> Sensors { get; set; }
        public List<ControlBoardSyncModel> ControlBoards { get; set; }
    }
}
