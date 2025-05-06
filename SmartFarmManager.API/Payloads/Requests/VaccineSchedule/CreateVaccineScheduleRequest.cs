using SmartFarmManager.Service.BusinessModels.VaccineSchedule;

namespace SmartFarmManager.API.Payloads.Requests.VaccineSchedule
{
    public class CreateVaccineScheduleRequest
    {
        public Guid VaccineId { get; set; }
        public Guid CageId { get; set; }
        public DateTime? Date { get; set; }
        public int Session { get; set; }

        public CreateVaccineScheduleModel MapToModel()
        {
            return new CreateVaccineScheduleModel
            {
                VaccineId = this.VaccineId,
                CageId = this.CageId,
                Date = this.Date,
                Session = this.Session,
            };
        }
    }

}
