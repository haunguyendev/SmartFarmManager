using Microsoft.EntityFrameworkCore;
using SmartFarmManager.DataAccessObject.Models;
using SmartFarmManager.Repository.Interfaces;
using SmartFarmManager.Service.BusinessModels;
using SmartFarmManager.Service.BusinessModels.VaccineSchedule;
using SmartFarmManager.Service.BusinessModels.VaccineScheduleLog;
using SmartFarmManager.Service.Helpers;
using SmartFarmManager.Service.Interfaces;
using SmartFarmManager.Service.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFarmManager.Service.Services
{
    public class VaccineScheduleService :IVaccineScheduleService
    {
        private readonly IUnitOfWork _unitOfWork;

        public VaccineScheduleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<VaccineScheduleResponse>> GetVaccineSchedulesAsync(
    Guid? stageId, DateTime? date, string status)
        {
            var query = _unitOfWork.VaccineSchedules
                .FindAll()
                .Include(vs => vs.Vaccine) // Join đến Vaccine để lấy tên
                .AsQueryable();

            // 1️⃣ Lọc theo StageId nếu có
            if (stageId.HasValue)
            {
                query = query.Where(vs => vs.StageId == stageId.Value);
            }

            // 2️⃣ Lọc theo Date nếu có
            if (date.HasValue)
            {
                query = query.Where(vs => vs.Date.Value.Date == date.Value.Date);
            }

            // 3️⃣ Lọc theo Status nếu có
            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(vs => vs.Status == status);
            }

            // 4️⃣ Thực thi truy vấn và ánh xạ vào response
            var result = await query.Select(vs => new VaccineScheduleResponse
            {
                VaccineScheduleId = vs.Id,
                StageId = vs.StageId,
                VaccineId = vs.VaccineId,
                VaccineName = vs.Vaccine.Name, // Lấy tên vaccine
                Date = vs.Date,
                Quantity = vs.Quantity,
                ApplicationAge = vs.ApplicationAge,
                TotalPrice = vs.ToltalPrice,
                Session = vs.Session,
                Status = vs.Status
            }).ToListAsync();

            return result;
        }
        public async Task<VaccineScheduleResponse> GetVaccineScheduleByIdAsync(Guid id)
        {
            var vaccineSchedule = await _unitOfWork.VaccineSchedules
                .FindByCondition(vs => vs.Id == id)
                .Include(vs => vs.Vaccine) // Lấy thông tin Vaccine
                .FirstOrDefaultAsync();

            if (vaccineSchedule == null) return null;

            return new VaccineScheduleResponse
            {
                VaccineScheduleId = vaccineSchedule.Id,
                VaccineId = vaccineSchedule.VaccineId,
                VaccineName = vaccineSchedule.Vaccine?.Name, // Tránh lỗi null
                StageId = vaccineSchedule.StageId,
                Date = vaccineSchedule.Date,
                Quantity = vaccineSchedule.Quantity,
                ApplicationAge = vaccineSchedule.ApplicationAge,
                TotalPrice = vaccineSchedule.ToltalPrice,
                Session = vaccineSchedule.Session,
                Status = vaccineSchedule.Status
            };
        }

        public async Task<VaccineScheduleWithLogsResponse> GetVaccineScheduleByTaskIdAsync(Guid taskId)
        {
            // Tìm VaccineScheduleLog dựa trên TaskId
            var vaccineScheduleLog = await _unitOfWork.VaccineScheduleLogs
                .FindByCondition(vsl => vsl.TaskId == taskId)
                .Include(vsl => vsl.Schedule)
                .ThenInclude(vs => vs.Vaccine)
                .FirstOrDefaultAsync();

            if (vaccineScheduleLog == null || vaccineScheduleLog.Schedule == null) return null;

            var vaccineSchedule = vaccineScheduleLog.Schedule;

            // Lấy tất cả log liên quan đến VaccineSchedule
            var logs = await _unitOfWork.VaccineScheduleLogs
                .FindByCondition(vsl => vsl.ScheduleId == vaccineSchedule.Id)
                .ToListAsync();

            return new VaccineScheduleWithLogsResponse
            {
                Id = vaccineSchedule.Id,
                VaccineId = vaccineSchedule.VaccineId,
                VaccineName = vaccineSchedule.Vaccine?.Name,
                StageId = vaccineSchedule.StageId,
                Date = vaccineSchedule.Date,
                Quantity = vaccineSchedule.Quantity,
                ApplicationAge = vaccineSchedule.ApplicationAge,
                TotalPrice = vaccineSchedule.ToltalPrice,
                Session = vaccineSchedule.Session,
                Status = vaccineSchedule.Status,
                Logs = logs.Select(log => new VaccineScheduleLogResponse
                {
                    Id = log.Id,
                    ScheduleId = log.ScheduleId,
                    Date = log.Date,
                    Notes = log.Notes,
                    Photo = log.Photo,
                    TaskId = log.TaskId
                }).ToList()
            };
        }


        public async Task<bool> CreateVaccineScheduleAsync(CreateVaccineScheduleModel model)
        {
            var today = DateTimeUtils.GetServerTimeInVietnamTime().Date;
            var tomorrow = today.AddDays(1);

            if (!model.Date.HasValue || model.Date.Value.Date < today)
            {
                throw new ArgumentException("Ngày tiêm phải là hôm nay hoặc trong tương lai.");
            }

            var vaccine = await _unitOfWork.Vaccines.FindByCondition(v => v.Id == model.VaccineId).FirstOrDefaultAsync();
            if (vaccine == null)
            {
                throw new ArgumentException($"Vaccine với ID {model.VaccineId} không tồn tại.");
            }

            var farmingBatch = await _unitOfWork.FarmingBatches
                .FindByCondition(fb => fb.CageId == model.CageId && fb.Status == FarmingBatchStatusEnum.Active)
                .Include(fb => fb.GrowthStages)
                .FirstOrDefaultAsync();

            if (farmingBatch == null)
            {
                throw new ArgumentException("Không tìm thấy lứa nuôi đang hoạt động trong chuồng đã chọn.");
            }
            var growthStage = farmingBatch.GrowthStages.FirstOrDefault(gs => gs.Status == GrowthStageStatusEnum.Active);
            if (growthStage == null)
            {
                throw new ArgumentException("Không tìm thấy giai đoạn phát triển đang hoạt động trong chuồng.");
            }

            var duplicate = await _unitOfWork.VaccineSchedules
                .FindByCondition(vs => vs.StageId == growthStage.Id &&
                                       vs.Date.HasValue &&
                                       vs.Date.Value.Date == model.Date.Value.Date &&
                                       vs.Session == model.Session)
                .AnyAsync();

            if (duplicate)
            {
                throw new ArgumentException("Đã có lịch tiêm trong cùng ngày và buổi đã chọn.");
            }

            var vaccineSchedule = new VaccineSchedule
            {
                Id = Guid.NewGuid(),
                VaccineId = model.VaccineId,
                StageId = growthStage.Id,
                Date = model.Date,
                Session = model.Session,
                Status = VaccineScheduleStatusEnum.Upcoming,
                Quantity = 0,
                ToltalPrice = 0,
            };

            await _unitOfWork.VaccineSchedules.CreateAsync(vaccineSchedule);

            if (model.Date.HasValue && (model.Date.Value.Date == today || model.Date.Value.Date == tomorrow))
            {
                var assignedStaff = await GetAssignedStaffForCage(model.CageId, model.Date.Value);
                if (assignedStaff == null)
                {
                    throw new Exception("Không có nhân viên được gán cho chuồng này vào ngày đã chọn.");
                }

                var vaccineTaskTypeId = await GetTaskTypeIdByName("Tiêm vắc xin");
                var vaccineTaskType = await _unitOfWork.TaskTypes.FindByCondition(tt => tt.Id == vaccineTaskTypeId).FirstOrDefaultAsync();

                var task = new DataAccessObject.Models.Task
                {
                    Id = Guid.NewGuid(),
                    TaskTypeId = vaccineTaskTypeId,
                    CageId = model.CageId,
                    AssignedToUserId = assignedStaff.Value,
                    CreatedByUserId = (await GetAdminId()) ?? Guid.Empty,
                    TaskName = $"Tiêm vắc xin: {vaccine.Name}",
                    PriorityNum = (int)(vaccineTaskType?.PriorityNum ?? 1),
                    Description = $"Tiêm vắc xin {vaccine.Name} cho giai đoạn {growthStage.Name}.",
                    DueDate = model.Date.Value.Date + GetSessionEndTime(model.Session),
                    Session = model.Session,
                    Status = TaskStatusEnum.Pending,
                    CreatedAt = DateTimeUtils.GetServerTimeInVietnamTime()
                };

                await _unitOfWork.Tasks.CreateAsync(task);
            }

            await _unitOfWork.CommitAsync();
            return true;
        }
        private async Task<Guid?> GetAssignedStaffForCage(Guid cageId, DateTime date)
        {
            return await _unitOfWork.CageStaffs
                .FindByCondition(cs => cs.CageId == cageId && cs.StaffFarm.Role.RoleName == "Staff Farm")
                .Include(cs => cs.StaffFarm)
                .Select(cs => (Guid?)cs.StaffFarmId)
                .FirstOrDefaultAsync();
        }

        private async Task<Guid?> GetAdminId()
        {
            return await _unitOfWork.Users
                .FindByCondition(u => u.Role.RoleName == "Admin Farm")
                .Select(u => (Guid?)u.Id)
                .FirstOrDefaultAsync();
        }

        private async Task<Guid> GetTaskTypeIdByName(string taskTypeName)
        {
            var taskType = await _unitOfWork.TaskTypes
                .FindByCondition(tt => tt.TaskTypeName == taskTypeName)
                .FirstOrDefaultAsync();

            if (taskType == null)
                throw new ArgumentException($"Không tìm thấy loại nhiệm vụ '{taskTypeName}'");

            return taskType.Id;
        }

        private TimeSpan GetSessionEndTime(int session)
        {
            return session switch
            {
                1 => SessionTime.Morning.End,
                2 => SessionTime.Noon.End,
                3 => SessionTime.Afternoon.End,
                4 => SessionTime.Evening.End,
                5 => SessionTime.Night.End,
                _ => TimeSpan.FromHours(18)
            };
        }

        public async Task<PagedResult<VaccineScheduleItemModel>> GetVaccineSchedulesAsync(VaccineScheduleFilterKeySearchModel filter)
        {
            var query = _unitOfWork.VaccineSchedules.FindAll(false)
                .Include(x=>x.Vaccine)
                .Include(x => x.Stage)
                .ThenInclude(x => x.FarmingBatch)
                .ThenInclude(x => x.Cage)
                .AsQueryable();

            if (!string.IsNullOrEmpty(filter.KeySearch)){
                query = query.Where(v => v.Vaccine.Name.Contains(filter.KeySearch)||
                v.Stage.Name.Contains(filter.KeySearch)||
                v.Stage.FarmingBatch.Name.Contains(filter.KeySearch));
            }

            if (filter.VaccineId.HasValue)
            {
                query = query.Where(v => v.VaccineId == filter.VaccineId.Value);
            }

            if (filter.StageId.HasValue)
            {
                query = query.Where(v => v.StageId == filter.StageId.Value);
            }

            if (filter.Date.HasValue)
            {
                query = query.Where(v => v.Date.Value.Date == filter.Date.Value.Date);
            }
            if(!string.IsNullOrEmpty(filter.Status))
            {
                query = query.Where(v => v.Status == filter.Status);
            }

            var totalItems = await query.CountAsync();

            var items = await query
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(v => new VaccineScheduleItemModel
                {
                    Id = v.Id,
                    VaccineId = v.VaccineId,
                    VaccineName=v.Vaccine.Name,
                    StageId = v.StageId,
                    StageName = v.Stage.Name,
                    FarmingBatchId=v.Stage.FarmingBatchId,
                    FarmingBatchName = v.Stage.FarmingBatch.Name,
                    CageId = v.Stage.FarmingBatch.CageId,
                    CageName = v.Stage.FarmingBatch.Cage.Name,
                    Date = v.Date,
                    Quantity = v.Quantity,
                    ApplicationAge = v.ApplicationAge,
                    ToltalPrice = v.ToltalPrice,
                    Session = v.Session,
                    Status = v.Status
                })
                .ToListAsync();

            return new PagedResult<VaccineScheduleItemModel>
            {
                Items = items,
                TotalItems = totalItems,
                PageSize = filter.PageSize,
                CurrentPage = filter.PageNumber,
                TotalPages = (int)Math.Ceiling((double)totalItems / filter.PageSize)
            };
        }

    }
}
