using SmartFarmManager.DataAccessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFarmManager.Service.Interfaces
{
    public interface IMedicationService
    {
        Task<Medication> CreateMedicationAsync(Medication medication);
        Task<IEnumerable<Medication>> GetAllMedicationsAsync();
    }

}
