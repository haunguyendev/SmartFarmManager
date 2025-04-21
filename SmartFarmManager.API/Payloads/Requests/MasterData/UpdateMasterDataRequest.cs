using SmartFarmManager.Service.BusinessModels.MasterData;
using System.ComponentModel.DataAnnotations;

namespace SmartFarmManager.API.Payloads.Requests.MasterData
{
    public class UpdateMasterDataRequest
    {


        [StringLength(20, ErrorMessage = "Unit cannot be longer than 20 characters.")]
        public string? Unit { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "UnitPrice must be greater than 0.")]
        public decimal? UnitPrice { get; set; }


        public MasterDataUpdateModel MapToModel()
        {
            return new MasterDataUpdateModel
            {
                Unit = Unit,
                UnitPrice = UnitPrice,
            };
        }
    }
}
