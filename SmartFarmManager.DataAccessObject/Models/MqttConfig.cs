﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace SmartFarmManager.DataAccessObject.Models;

public partial class MqttConfig : EntityBase
{

    public int Port { get; set; }

    public string BrokerAddress { get; set; }

    public int QoS { get; set; }

    public int KeepAlive { get; set; }

    public bool CleanSession { get; set; }

    public string UserName { get; set; }

    public string Password { get; set; }

    public string WillMessage { get; set; }

    public bool UseTls { get; set; }
}