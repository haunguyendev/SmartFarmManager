﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace SmartFarmManager.DataAccessObject.Models;

public partial class VaccineSchedule : EntityBase
{

    public Guid VaccineId { get; set; }

    public Guid StageId { get; set; }

    public DateTime? Date { get; set; }

    public int? Quantity { get; set; }
    public int? ApplicationAge { get; set; }
    public decimal? ToltalPrice { get; set; }
    public int Session { get; set; }

    public string Status { get; set; }

    public virtual GrowthStage Stage { get; set; }

    public virtual Vaccine Vaccine { get; set; }

    public virtual ICollection<VaccineScheduleLog> VaccineScheduleLogs { get; set; } = new List<VaccineScheduleLog>();
}