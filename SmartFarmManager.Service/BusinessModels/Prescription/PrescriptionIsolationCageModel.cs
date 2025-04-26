using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFarmManager.Service.BusinessModels.Prescription
{
    public class PrescriptionIsolationCageModel
    {
        // Prescription fields
        public Guid Id { get; set; }
        public DateTime? PrescribedDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Status { get; set; }
        public int QuantityAnimal { get; set; }
        public decimal? Price { get; set; }
        public string Notes { get; set; }

        // MedicalSymptom fields
        public Guid MedicalSymptomId { get; set; }
        public string Diagnosis { get; set; }
        public string SymptomNotes { get; set; }

        // Cage fields
        public Guid CageId { get; set; }
        public string CageName { get; set; }
    }
}
