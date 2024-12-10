using Microsoft.EntityFrameworkCore;
using SmartFarmManager.DataAccessObject.Models;
using SmartFarmManager.Repository.Interfaces;
using SmartFarmManager.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFarmManager.Service.Services
{
    public class MedicationService : IMedicationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MedicationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Medication> CreateMedicationAsync(Medication medication)
        {
            await _unitOfWork.Medication.CreateAsync(medication);
            await _unitOfWork.CommitAsync();
            return medication;
        }

        public async Task<IEnumerable<Medication>> GetAllMedicationsAsync()
        {
            return await _unitOfWork.Medication.FindAll().ToListAsync();
        }
    }

}
