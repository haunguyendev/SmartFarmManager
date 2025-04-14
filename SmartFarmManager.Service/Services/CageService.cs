﻿using MailKit;
using Microsoft.EntityFrameworkCore;
using SmartFarmManager.DataAccessObject.Models;
using SmartFarmManager.Repository.Interfaces;
using SmartFarmManager.Service.BusinessModels;
using SmartFarmManager.Service.BusinessModels.Cages;
using SmartFarmManager.Service.BusinessModels.FarmingBatch;
using SmartFarmManager.Service.BusinessModels.GrowthStage;
using SmartFarmManager.Service.BusinessModels.Prescription;
using SmartFarmManager.Service.BusinessModels.Task;
using SmartFarmManager.Service.BusinessModels.TaskDaily;
using SmartFarmManager.Service.BusinessModels.VaccineSchedule;
using SmartFarmManager.Service.Helpers;
using SmartFarmManager.Service.Interfaces;
using SmartFarmManager.Service.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace SmartFarmManager.Service.Services
{
    public class CageService : ICageService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CageService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PagedResult<CageResponseModel>> GetCagesAsync(CageFilterModel request)
        {
            // Lấy dữ liệu ban đầu từ UnitOfWork
            var query = _unitOfWork.Cages.FindAll(false, x => x.Farm)
                .Include(c => c.CageStaffs)
                .ThenInclude(cs => cs.StaffFarm)
                .Include(c => c.FarmingBatches).ThenInclude(c => c.GrowthStages)
                .Include(c => c.CageStaffs)
                .ThenInclude(cs => cs.StaffFarm)
                .ThenInclude(sf => sf.Role)
                .AsQueryable();

            query = query.Where(c => c.CageStaffs.Any(cs => cs.StaffFarm.Role.RoleName == "Staff Farm"));
            // Áp dụng các bộ lọc
            if (!string.IsNullOrEmpty(request.PenCode))
            {
                query = query.Where(c => c.PenCode.Contains(request.PenCode));
            }


            // Áp dụng bộ lọc Name
            if (!string.IsNullOrEmpty(request.Name))
            {
                query = query.Where(c => c.Name.Contains(request.Name));
            }

            // Tìm kiếm tổng hợp với SearchKey
            if (!string.IsNullOrEmpty(request.SearchKey))
            {
                query = query.Where(c =>
                    c.PenCode.Contains(request.SearchKey) ||
                    c.Name.Contains(request.SearchKey) ||
                    c.Location.Contains(request.SearchKey) ||
                    c.CageStaffs.FirstOrDefault().StaffFarm.FullName.Contains(request.SearchKey));
            }

            if (request.HasFarmingBatch.HasValue)
            {
                if (request.HasFarmingBatch.Value)
                {
                    query = query.Where(c => c.FarmingBatches.Any(fb =>
                        fb.StartDate < DateTimeUtils.GetServerTimeInVietnamTime() &&
                        fb.CompleteAt == null &&
                        fb.Status == FarmingBatchStatusEnum.Active));
                }
                else
                {
                    query = query.Where(c => !c.FarmingBatches.Any(fb => fb.Status == FarmingBatchStatusEnum.Active));
                }
            }
            // Đếm tổng số bản ghi (chạy trên SQL)
            var totalCount = await query.CountAsync();

            // Phân trang và chọn dữ liệu cần thiết (chạy trên SQL)
            var items = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(c => new CageResponseModel
                {
                    Id = c.Id,
                    PenCode = c.PenCode,
                    FarmId = c.FarmId,
                    Name = c.Name,
                    Area = c.Area,
                    Location = c.Location,
                    Capacity = c.Capacity,
                    BoardCode = c.BoardCode,
                    BoardStatus = c.BoardStatus,
                    CreatedDate = c.CreatedDate,
                    CameraUrl = c.CameraUrl,
                    StaffId = c.CageStaffs.FirstOrDefault().StaffFarmId,
                    StaffName = c.CageStaffs.FirstOrDefault().StaffFarm.FullName,
                    IsSolationCage = c.IsSolationCage,
                    // Lấy thông tin FarmingBatch phù hợp
                    FarmingBatch = c.FarmingBatches
                .Where(fb => fb.StartDate < DateTimeUtils.GetServerTimeInVietnamTime() && fb.CompleteAt == null && fb.Status == FarmingBatchStatusEnum.Active)
                .Select(fb => new FarmingBatchModel
                {
                    Id = fb.Id,
                    Name = fb.Name,
                    StartDate = fb.StartDate,
                    CompleteAt = fb.CompleteAt,
                    EndDate = fb.EndDate,
                    Status = fb.Status,
                    CleaningFrequency = fb.CleaningFrequency,
                    Quantity = fb.Quantity,
                    DeadQuantity = fb.DeadQuantity,
                    GrowthStageDetails = fb.GrowthStages.Where(gs =>
                        gs.AgeStartDate.HasValue &&
                        gs.AgeEndDate.HasValue &&
                        gs.AgeStartDate.Value.Date <= DateTimeUtils.GetServerTimeInVietnamTime().Date &&
                        gs.AgeEndDate.Value.Date >= DateTimeUtils.GetServerTimeInVietnamTime().Date)
                        .Select(gs => new GrowthStageDetailModel
                        {
                            Id = gs.Id,
                            Name = gs.Name,
                            WeightAnimal = gs.WeightAnimal,
                            Quantity = gs.Quantity,
                            AgeStart = gs.AgeStart,
                            AgeEnd = gs.AgeEnd,
                            AgeStartDate = gs.AgeStartDate,
                            AgeEndDate = gs.AgeEndDate,
                            Status = gs.Status,
                            AffectQuantity = gs.AffectedQuantity,
                            DeadQuantity = gs.DeadQuantity,
                            RecommendedWeightPerSession = gs.RecommendedWeightPerSession,
                            WeightBasedOnBodyMass = gs.WeightBasedOnBodyMass,
                        })
                        .FirstOrDefault()
                })
                .FirstOrDefault()

                })
                .ToListAsync();

            // Kết quả trả về dạng phân trang
            return new PagedResult<CageResponseModel>
            {
                Items = items,
                TotalItems = totalCount,
                CurrentPage = request.PageNumber,
                PageSize = request.PageSize,
                TotalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize),
                HasNextPage = request.PageNumber < (int)Math.Ceiling(totalCount / (double)request.PageSize),
                HasPreviousPage = request.PageNumber > 1,
            };
        }

        public async Task<CageDetailModel> GetCageByIdAsync(Guid cageId)
        {
            // Lấy dữ liệu từ repository
            var cage = await _unitOfWork.Cages
                .FindByCondition(x => x.Id == cageId && x.CageStaffs.Any(cs => cs.StaffFarm.Role.RoleName == "Staff Farm"), false, c => c.Farm)
                .Include(c => c.CageStaffs)
                .ThenInclude(cs => cs.StaffFarm)
                .ThenInclude(sf => sf.Role)
                .FirstOrDefaultAsync();
            // Xử lý khi không tìm thấy cage
            if (cage == null || cage.IsDeleted)
            {
                throw new KeyNotFoundException("Cage not found.");
            }

            // Trả về DTO
            return new CageDetailModel
            {
                Id = cage.Id,
                PenCode = cage.PenCode,
                FarmId = cage.FarmId,
                Name = cage.Name,
                Area = cage.Area,
                Location = cage.Location,
                Capacity = cage.Capacity,
                BoardCode = cage.BoardCode,
                BoardStatus = cage.BoardStatus,
                CreatedDate = cage.CreatedDate,
                CameraUrl = cage.CameraUrl,
                StaffId = cage.CageStaffs.FirstOrDefault().StaffFarmId, // Lấy StaffId từ CageStaff
                StaffName = cage.CageStaffs.FirstOrDefault().StaffFarm.FullName
            };
        }


        public async Task<List<CageResponseModel>> GetUserCagesAsync(Guid userId)
        {
            // Lấy danh sách cages mà user thuộc
            var userCages = await _unitOfWork.CageStaffs
                .FindByCondition(cs => cs.StaffFarmId == userId && !cs.Cage.IsDeleted)
                .Include(cs => cs.Cage)
                .Select(cs => new CageResponseModel
                {
                    Id = cs.Cage.Id,
                    PenCode = cs.Cage.PenCode,
                    FarmId = cs.Cage.FarmId,
                    Name = cs.Cage.Name,
                    Area = cs.Cage.Area,
                    Location = cs.Cage.Location,
                    Capacity = cs.Cage.Capacity,
                    BoardCode = cs.Cage.BoardCode,
                    BoardStatus = cs.Cage.BoardStatus,
                    CreatedDate = cs.Cage.CreatedDate,
                    CameraUrl = cs.Cage.CameraUrl
                })
                .ToListAsync();

            // Kiểm tra nếu không có cage nào
            if (!userCages.Any())
            {
                throw new ArgumentException($"No cages found for user with ID {userId}.");
            }

            return userCages;
        }

        public async Task<Guid> CreateCageAsync(CageModel model)
        {
            var cage = new Cage
            {
                FarmId = model.FarmId,
                Name = model.Name,
                Area = model.Area,
                Capacity = model.Capacity,
                Location = model.Location,
                CreatedDate = DateTimeUtils.GetServerTimeInVietnamTime(),
            };

            var id = await _unitOfWork.Cages.CreateAsync(cage);
            await _unitOfWork.CommitAsync();
            return id;
        }


        public async Task<IEnumerable<CageModel>> GetAllCagesAsync(string? search)
        {
            var cages = await _unitOfWork.Cages.FindAllAsync(c => string.IsNullOrEmpty(search) || c.Name.Contains(search));

            return cages.Select(c => new CageModel
            {
                Id = c.Id,
                FarmId = c.FarmId,
                Name = c.Name,
                Area = c.Area,
                Capacity = c.Capacity,
                Location = c.Location,
            });
        }

        public async Task<bool> UpdateCageAsync(Guid id, CageModel model)
        {
            var cage = await _unitOfWork.Cages.GetByIdAsync(id);
            if (cage == null) return false;

            cage.FarmId = model.FarmId;
            cage.Name = model.Name;
            cage.Area = model.Area;
            cage.Capacity = model.Capacity;
            cage.Location = model.Location;
            cage.ModifiedDate = DateTimeUtils.GetServerTimeInVietnamTime();

            await _unitOfWork.Cages.UpdateAsync(cage);
            await _unitOfWork.CommitAsync();
            return true;
        }

        public async Task<bool> DeleteCageAsync(Guid id)
        {
            var cage = await _unitOfWork.Cages.GetByIdAsync(id);
            if (cage == null) return false;

            cage.IsDeleted = true;
            cage.DeletedDate = DateTimeUtils.GetServerTimeInVietnamTime();

            await _unitOfWork.Cages.UpdateAsync(cage);
            await _unitOfWork.CommitAsync();
            return true;
        }

        public async Task<CageIsolationResponseModel> GetPrescriptionsWithTasksAsync()
        {
            // Step 1: Retrieve the cage and its tasks
            var cage = await _unitOfWork.Cages.FindByCondition(c => c.IsSolationCage == true).Include(c => c.Tasks).FirstOrDefaultAsync();
            if (cage == null) throw new KeyNotFoundException("Cage not found.");
            if (!cage.IsSolationCage) throw new InvalidOperationException("Cage is not an isolation cage.");

            // Step 2: Filter tasks for the current day
            var today = DateTimeUtils.GetServerTimeInVietnamTime().Date;
            var todayTasks = cage.Tasks.Where(t => t.DueDate.HasValue && t.DueDate.Value.Date == today).ToList();

            // Step 3: Extract distinct Prescription IDs
            var distinctPrescriptionIds = todayTasks
                .Where(t => t.PrescriptionId.HasValue)
                .Select(t => t.PrescriptionId.Value)
                .Distinct()
                .ToList();

            if (!distinctPrescriptionIds.Any())
                return MapCageToResponse(cage, new List<PrescriptionResponseModel>());

            // Step 4: Retrieve prescriptions using FindByCondition
            var prescriptions = await _unitOfWork.Prescription
                .FindByCondition(p => distinctPrescriptionIds.Contains(p.Id))
                .Include(p => p.MedicalSymtom)
                .ToListAsync();

            // Step 5: Iterate through prescriptions and filter tasks based on DueDate, then sort by Session
            var prescriptionResponses = new List<PrescriptionResponseModel>();
            foreach (var prescription in prescriptions)
            {
                var prescriptionTasks = await _unitOfWork.Tasks
                    .FindByCondition(t => t.PrescriptionId == prescription.Id && t.DueDate.HasValue && t.DueDate.Value.Date == today)
                    .OrderBy(t => t.Session) // Sort tasks by Session in ascending order
                    .ToListAsync();
                var farmingBatchAnimal = await _unitOfWork.FarmingBatches.FindByCondition(fb => fb.Id == prescription.MedicalSymtom.FarmingBatchId).Include(fb => fb.Cage).FirstOrDefaultAsync();

                prescriptionResponses.Add(new PrescriptionResponseModel
                {
                    Id = prescription.Id,
                    MedicalSymptomId = prescription.MedicalSymtomId,
                    CageId = prescription.CageId,
                    PrescribedDate = prescription.PrescribedDate,
                    EndDate = prescription.EndDate,
                    Notes = prescription.Notes,
                    QuantityAnimal = prescription.QuantityAnimal,
                    RemainingQuantity = prescription.RemainingQuantity,
                    Status = prescription.Status,
                    DaysToTake = prescription.DaysToTake,
                    Price = prescription.Price,
                    cageAnimal = farmingBatchAnimal.Cage.Name,
                    Diagnosis = prescription.MedicalSymtom.Diagnosis,
                    Tasks = prescriptionTasks.Select(t => new TaskResponseModel
                    {
                        Id = t.Id,
                        TaskTypeId = t.TaskTypeId,
                        CageId = t.CageId,
                        AssignedToUserId = t.AssignedToUserId,
                        CreatedByUserId = t.CreatedByUserId,
                        TaskName = t.TaskName,
                        PriorityNum = t.PriorityNum,
                        Description = t.Description,
                        DueDate = t.DueDate,
                        Status = t.Status,
                        Session = t.Session, // Sorted by Session
                        IsWarning = t.IsWarning,
                        MedicalSymptomId = t.MedicalSymptomId,
                        CompletedAt = t.CompletedAt,
                        CreatedAt = t.CreatedAt,
                        IsTreatmentTask = t.IsTreatmentTask,
                        PrescriptionId = t.PrescriptionId
                    }).ToList()
                });
            }

            // Step 6: Return the CageResponseModel with prescriptions and tasks
            return MapCageToResponse(cage, prescriptionResponses);
        }

        private CageIsolationResponseModel MapCageToResponse(Cage cage, List<PrescriptionResponseModel> prescriptions)
        {
            return new CageIsolationResponseModel
            {
                Id = cage.Id,
                PenCode = cage.PenCode,
                FarmId = cage.FarmId,
                Name = cage.Name,
                Area = cage.Area,
                Location = cage.Location,
                Capacity = cage.Capacity,
                BoardCode = cage.BoardCode,
                BoardStatus = cage.BoardStatus,
                CreatedDate = cage.CreatedDate,
                ModifiedDate = cage.ModifiedDate,
                IsDeleted = cage.IsDeleted,
                DeletedDate = cage.DeletedDate,
                CameraUrl = cage.CameraUrl,
                ChannelId = cage.ChannelId,
                IsSolationCage = cage.IsSolationCage,
                Prescriptions = prescriptions
            };
        }

    }
}
