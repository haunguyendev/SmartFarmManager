﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace SmartFarmManager.DataAccessObject.Models;

public partial class Farm : EntityBase
{

    public string FarmCode { get; set; }

    public string Name { get; set; }

    public string Address { get; set; }

    public string PhoneNumber { get; set; }

    public string Email { get; set; }

    public double Area { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedDate { get; set; }

    public string Macaddress { get; set; }

    public virtual ICollection<ElectricityLog> ElectricityLogs { get; set; } = new List<ElectricityLog>();

    public virtual ICollection<FarmAdmin> FarmAdmins { get; set; } = new List<FarmAdmin>();

    public virtual ICollection<FarmCamera> FarmCameras { get; set; } = new List<FarmCamera>();

    public virtual ICollection<FarmSubscription> FarmSubscriptions { get; set; } = new List<FarmSubscription>();

    public virtual ICollection<FoodStack> FoodStacks { get; set; } = new List<FoodStack>();

    public virtual ICollection<GrowthStage> GrowthStages { get; set; } = new List<GrowthStage>();

    public virtual ICollection<WaterLog> WaterLogs { get; set; } = new List<WaterLog>();
}