using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFarmManager.Service.BusinessModels.VaccineSchedule
{
    public class CreateVaccineScheduleModel
    {
        public Guid VaccineId { get; set; }
        public Guid CageId  { get; set; }
        public DateTime? Date { get; set; }
        public int Session { get; set; }
    }
}
