﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace SmartFarmManager.DataAccessObject.Models;

public partial class Prescription : EntityBase
{

    public Guid RecordId { get; set; }

    public DateOnly? PrescribedDate { get; set; }

    public string CaseType { get; set; }

    public string Notes { get; set; }

    public decimal? Price { get; set; }

    public virtual ICollection<HealthLog> HealthLogs { get; set; } = new List<HealthLog>();

    public virtual ICollection<PrescriptionMedication> PrescriptionMedications { get; set; } = new List<PrescriptionMedication>();

    public virtual MedicalSymptom Record { get; set; }
}