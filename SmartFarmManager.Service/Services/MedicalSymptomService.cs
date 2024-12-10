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
    public class MedicalSymptomService : IMedicalSymptomService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MedicalSymptomService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<MedicalSymptom> CreateMedicalSymptomAsync(MedicalSymptom medicalSymptom)
        {
            // Ensure Diagnosis and Treatment are null for doctors to update later
            medicalSymptom.Diagnosis = null;
            medicalSymptom.Treatment = null;

            await _unitOfWork.MedicalSymptom.CreateAsync(medicalSymptom);
            await _unitOfWork.CommitAsync();

            return medicalSymptom;
        }

        public async Task<MedicalSymptom?> GetMedicalSymptomByIdAsync(Guid id)
        {
            return await _unitOfWork.MedicalSymptom.GetByIdAsync(id);
        }

        public async Task<IEnumerable<MedicalSymptom>> GetMedicalSymptomsAsync(string? status)
        {
            if (string.IsNullOrEmpty(status))
            {
                return await _unitOfWork.MedicalSymptom.FindAll().ToListAsync();
            }

            return await _unitOfWork.MedicalSymptom
                .FindByCondition(ms => ms.Status == status).ToListAsync();
        }

        public async Task<bool> UpdateMedicalSymptomAsync(MedicalSymptom updatedSymptom)
        {
            var existingSymptom = await _unitOfWork.MedicalSymptom.GetByIdAsync(updatedSymptom.Id);

            if (existingSymptom == null)
            {
                return false;
            }

            existingSymptom.Diagnosis = updatedSymptom.Diagnosis;
            existingSymptom.Treatment = updatedSymptom.Treatment;
            existingSymptom.Status = updatedSymptom.Status;
            existingSymptom.Notes = updatedSymptom.Notes;

            await _unitOfWork.MedicalSymptom.UpdateAsync(existingSymptom);
            await _unitOfWork.CommitAsync();

            return true;
        }
    }
}
