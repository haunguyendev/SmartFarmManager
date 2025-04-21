using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFarmManager.Service.BusinessModels.Webhook
{
    public class NotificationChangeOfFarmModel
    {
        public Guid FarmId { get; set; }
        public string Log { get; set; }
    }
}
