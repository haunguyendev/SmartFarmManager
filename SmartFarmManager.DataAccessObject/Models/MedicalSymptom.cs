﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace SmartFarmManager.DataAccessObject.Models;

public partial class MedicalSymptom : EntityBase
{

    public Guid FarmingBatchId { get; set; }
    public Guid? PrescriptionId { get; set; }

    public string Diagnosis { get; set; }

    public string Status { get; set; }

    public int? AffectedQuantity { get; set; }
    public bool IsEmergency { get; set; } = false;
    public int? QuantityInCage { get; set; }


    public string Notes { get; set; }
    public DateTime? CreateAt {get;set;}

    public DateTime? FirstReminderSentAt { get; set; }
    public DateTime? SecondReminderSentAt { get; set; }
    public Guid? DiseaseId { get; set; }
    public virtual FarmingBatch FarmingBatch { get; set; }

    public  virtual Disease Disease { get; set; }
    public virtual ICollection<Picture> Pictures { get; set; } = new List<Picture>();

    public virtual ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
    public virtual ICollection<MedicalSymtomDetail> MedicalSymptomDetails { get; set; } = new List<MedicalSymtomDetail>();
}