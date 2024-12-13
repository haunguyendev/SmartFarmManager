﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace SmartFarmManager.DataAccessObject.Models;

public partial class FarmingBatch : EntityBase
{

    public Guid TemplateId { get; set; }

    public Guid CageId { get; set; }

    public string Name { get; set; }

    public string Species { get; set; }

    public DateTime? StartDate { get; set; }
    public DateTime? CompleteAt { get; set; }
    public string Status { get; set; }

    public int CleaningFrequency { get; set; }

    public int? Quantity { get; set; }

    public int FarmId { get; set; }

    public virtual ICollection<AnimalSale> AnimalSales { get; set; } = new List<AnimalSale>();

    public virtual Cage Cage { get; set; }

    public virtual ICollection<GrowthStage> GrowthStages { get; set; } = new List<GrowthStage>();

    public virtual ICollection<MedicalSymptom> MedicalSymptoms { get; set; } = new List<MedicalSymptom>();

    public virtual AnimalTemplate Template { get; set; }
}