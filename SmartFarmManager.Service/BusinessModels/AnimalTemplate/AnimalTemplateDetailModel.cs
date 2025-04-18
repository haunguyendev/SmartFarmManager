using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFarmManager.Service.BusinessModels.AnimalTemplate
{
    public class AnimalTemplateItemModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Species { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
        public bool IsDeleted { get; set; }

        // Thêm các trường mới
        public List<GrowthStageTemplateModel>? GrowthStageTemplates { get; set; } = new List<GrowthStageTemplateModel>();
        public List<VaccineTemplateModel>? VaccineTemplates { get; set; } = new List<VaccineTemplateModel>();
    }

    public class GrowthStageTemplateModel
    {
        public Guid Id { get; set; }
        public string StageName { get; set; }
        public decimal? WeightAnimal { get; set; }
        public int? AgeStart { get; set; }
        public int? AgeEnd { get; set; }
        public string Notes { get; set; }
        public List<FoodTemplateModel>? FoodTemplates { get; set; } = new List<FoodTemplateModel>();
        public List<TaskDailyTemplateModel> TaskDailyTemplates { get; set; } = new List<TaskDailyTemplateModel>();
    }

    public class FoodTemplateModel
    {
        public Guid Id { get; set; }
        public string FoodType { get; set; }
        public decimal? WeightBasedOnBodyMass { get; set; }
    }

    public class VaccineTemplateModel
    {
        public Guid Id { get; set; }
        public string VaccineName { get; set; }
        public string ApplicationMethod { get; set; }
        public int? ApplicationAge { get; set; }
        public int Session { get; set; }
    }

    public class TaskDailyTemplateModel
    {
        public Guid Id { get; set; }
        public string TaskName { get; set; }
        public string Description { get; set; }
        public int Session { get; set; }
    }
}
