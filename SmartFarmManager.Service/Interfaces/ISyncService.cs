﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFarmManager.Service.Interfaces
{
    public interface ISyncService
    {
        System.Threading.Tasks.Task SyncFarmFromExternalAsync(Guid farmId);
    }

}
