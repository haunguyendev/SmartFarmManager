using SmartFarmManager.Service.BusinessModels.Farm;
using System.ComponentModel.DataAnnotations;

namespace SmartFarmManager.API.Payloads.Requests.Farm
{
    public class CreateFarmRequest
    {
        [Required(ErrorMessage = "Tên trang trại không được để trống")]
        public string? Name { get; set; }
        public string? FarmCode { get; set; }
        public string? Address { get; set; }
        [Required(ErrorMessage = "Diện tích không được để trống")]
        public double? Area { get; set; }
        public string? PhoneNumber { get; set; }
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        public string? Email { get; set; }
    }

    public class UpdateFarmRequest
    {
        public Guid? ExternalId { get; set; }
        public string? FarmCode { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")] 
        public string? Email { get; set; }
        public double? Area { get; set; }
   

        public FarmUpdateModel MapToModel() => new FarmUpdateModel
        {
            ExternalId = ExternalId,
            FarmCode = FarmCode,
            Name = Name,
            Address = Address,
            PhoneNumber = PhoneNumber,
            Email = Email,
            Area = Area
        };
    }

    public class UpdateExternalIdRequest
    {
        [Required(ErrorMessage = "ExternalId không được để trống")]
        public Guid? ExternalId { get; set; }
    }
}