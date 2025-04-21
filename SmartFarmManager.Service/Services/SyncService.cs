using Microsoft.EntityFrameworkCore;
using SmartFarmManager.DataAccessObject.Models;
using SmartFarmManager.Repository.Interfaces;
using SmartFarmManager.Service.ExternalClient;
using SmartFarmManager.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFarmManager.Service.Services
{
    public class SyncService:ISyncService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ExternalFarmApiClient _externalFarmApiClient;
        public SyncService(IUnitOfWork unitOfWork, ExternalFarmApiClient externalFarmApiClient)
        {
            _unitOfWork = unitOfWork;
            _externalFarmApiClient = externalFarmApiClient;
        }

        public async System.Threading.Tasks.Task SyncFarmFromExternalAsync( Guid farmId)
        {
            var farmExisting = await _unitOfWork.Farms.FindByCondition(x => x.Id == farmId).FirstOrDefaultAsync();
            if(farmExisting == null)
            {
                throw new Exception($"FarmId: {farmId} không tồn tại!");
            }

            var externalFarm = await _externalFarmApiClient.GetFarmDataAsync(farmExisting.FarmCode);
            if (externalFarm == null)
                throw new Exception("Không thể lấy dữ liệu từ API bên thứ ba.");

            // Step 1: Farm
            var farm = await _unitOfWork.Farms
                .FindByCondition(f => f.FarmCode == externalFarm.FarmCode)
                .FirstOrDefaultAsync();

            if (farm == null)
            {
                farm = new Farm
                {
                    Id = Guid.NewGuid(),
                    FarmCode = externalFarm.FarmCode,
                    CreatedDate = DateTime.UtcNow
                };
                await _unitOfWork.Farms.CreateAsync(farm);
                await _unitOfWork.CommitAsync();
            }

            farm.Name = externalFarm.Name;
            farm.Address = externalFarm.Address;
            farm.PhoneNumber = externalFarm.PhoneNumber;
            farm.Email = externalFarm.Email;
            farm.Area = externalFarm.Area;
            farm.Macaddress = externalFarm.MacAddress ?? "";
            farm.ModifiedDate = DateTime.UtcNow;
            await _unitOfWork.Farms.UpdateAsync(farm);
            await _unitOfWork.CommitAsync();

            // Step 2: Cage & Sensor & ControlBoard
            foreach (var pen in externalFarm.Pens)
            {
                var cage = await _unitOfWork.Cages
                    .FindByCondition(c => c.PenCode == pen.PenCode && c.Farm.FarmCode == farm.FarmCode).Include(c => c.Farm)
                    .FirstOrDefaultAsync();

                if (cage == null)
                {
                    cage = new Cage
                    {
                        Id = Guid.NewGuid(),
                        PenCode = pen.PenCode,
                        Name = pen.Name,
                        CameraUrl = pen.CameraUrl,
                        ChannelId = pen.ChannelId,
                        BoardCode = pen.ControlBoards.Where(x=>x.PenId==pen.Id).Select(x=>x.ControlBoardCode).FirstOrDefault(),                       
                        IsDeleted = false,
                        FarmId = await _unitOfWork.Farms.FindByCondition(x => x.FarmCode == farm.FarmCode).Select(x => x.Id).FirstOrDefaultAsync(),
                        CreatedDate = DateTime.UtcNow
                    };
                     await _unitOfWork.Cages.CreateAsync(cage);
                    await _unitOfWork.CommitAsync();
                }

                cage.Name = pen.Name;
                cage.CameraUrl = pen.CameraUrl;
                cage.ChannelId = pen.ChannelId;
                cage.ModifiedDate = DateTime.UtcNow;
                await _unitOfWork.Cages.UpdateAsync(cage);
                await _unitOfWork.CommitAsync();

                // Sensors
                foreach (var sensor in pen.Sensors)
                {
                    var pin = int.Parse(sensor.PinCode);

                    var entity = await _unitOfWork.Sensors
                        .FindByCondition(s =>
                            s.SensorCode == sensor.SensorCode &&
                            s.PinCode == pin &&
                            s.Cage.PenCode == cage.PenCode)
                        .Include(s => s.Cage)
                        .FirstOrDefaultAsync();

                    if (entity == null)
                    {
                        entity = new Sensor
                        {
                            Id = Guid.NewGuid(),
                            SensorCode = sensor.SensorCode,
                            Name=sensor.Name,
                            NodeId = sensor.NodeId,
                            PinCode=pin,
                            SensorTypeId=await _unitOfWork.SensorTypes.FindByCondition(x=>x.Name==sensor.SensorTypeName).Select(x=>x.Id).FirstOrDefaultAsync(),
                            CageId = await _unitOfWork.Cages.FindByCondition(x=>x.PenCode==pen.PenCode).Select(x=>x.Id).FirstOrDefaultAsync(),
                            CreatedDate = DateTime.UtcNow,
                            IsDeleted = false,
                        };
                        await _unitOfWork.Sensors.CreateAsync(entity);
                        await _unitOfWork.CommitAsync();
                    }

                    entity.Name = sensor.Name;
                    entity.NodeId = sensor.NodeId;
                    entity.PinCode = pin;
                    entity.SensorTypeId = await _unitOfWork.SensorTypes.FindByCondition(x => x.Name == sensor.SensorTypeName).Select(x => x.Id).FirstOrDefaultAsync();
                    entity.ModifiedDate = DateTime.UtcNow;
                    await _unitOfWork.Sensors.UpdateAsync(entity);
                    await _unitOfWork.CommitAsync();
                }

                // ControlBoards
                //foreach (var board in pen.ControlBoards)
                //{
                //    var controlBoard = await _unitOfWork.ControlBoards
                //        .FindByCondition(c => c.ControlBoardCode == board.ControlBoardCode && c.PenId == cage.Id)
                //        .FirstOrDefaultAsync();

                //    if (controlBoard == null)
                //    {
                //        controlBoard = new ControlBoard
                //        {
                //            Id = Guid.NewGuid(),
                //            ControlBoardCode = board.ControlBoardCode,
                //            PenId = cage.Id,
                //            CreatedDate = DateTime.UtcNow
                //        };
                //        _context.ControlBoards.Add(controlBoard);
                //    }

                //    controlBoard.Name = board.Name;
                //    controlBoard.PinCode = board.PinCode;
                //    controlBoard.ControlBoardTypeId = Guid.Parse(board.ControlBoardTypeId);
                //    controlBoard.ModifiedDate = DateTime.UtcNow;
                //}
            }

            await _unitOfWork.CommitAsync();
        }

    }
}
