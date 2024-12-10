using SmartFarmManager.DataAccessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFarmManager.Service.Interfaces
{
    public interface IMedicalSymptomService
    {
        Task<MedicalSymptom> CreateMedicalSymptomAsync(MedicalSymptom medicalSymptom);
        Task<MedicalSymptom?> GetMedicalSymptomByIdAsync(Guid id);
        Task<IEnumerable<MedicalSymptom>> GetMedicalSymptomsAsync(string? status);
        Task<bool> UpdateMedicalSymptomAsync(MedicalSymptom updatedSymptom);
    }
}
