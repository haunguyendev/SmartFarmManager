﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace SmartFarmManager.DataAccessObject.Models;

public partial class Cage : EntityBase
{
    public string PenCode { get; set; }

    public string Name { get; set; }

    public double Area { get; set; }

    public string Location { get; set; }

    public int Capacity { get; set; }

    public string AnimalType { get; set; }

    public string BoardCode { get; set; }

    public bool BoardStatus { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedDate { get; set; }

    public string CameraUrl { get; set; }

    public int ChannelId { get; set; }

    public virtual ICollection<CageStaff> CageStaffs { get; set; } = new List<CageStaff>();

    public virtual ICollection<ControlBoard> ControlBoards { get; set; } = new List<ControlBoard>();

    public virtual ICollection<FarmingBatch> FarmingBatches { get; set; } = new List<FarmingBatch>();

    public virtual ICollection<Sensor> Sensors { get; set; } = new List<Sensor>();

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}