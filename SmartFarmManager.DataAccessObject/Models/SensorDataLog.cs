﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace SmartFarmManager.DataAccessObject.Models;

public partial class SensorDataLog : EntityBase
{

    public Guid SensorId { get; set; }

    public string Data { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public bool IsWarning { get; set; }

    public virtual Sensor Sensor { get; set; }
}