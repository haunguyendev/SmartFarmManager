﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace SmartFarmManager.DataAccessObject.Models;

public partial class ControlBoardType : EntityBase
{

    public string Name { get; set; } // Name of the control board type

    public string Description { get; set; }

    public virtual ICollection<ControlBoard> ControlBoards { get; set; } = new List<ControlBoard>();
}