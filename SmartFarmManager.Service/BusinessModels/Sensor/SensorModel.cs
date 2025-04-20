using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFarmManager.Service.BusinessModels.Sensor
{
    public class SensorModel
    {
        public Guid SensorId { get; set; }  // Mã sensor
        public string SensorCode { get; set; }  // Mã sensor
        public string Name { get; set; }  // Tên sensor
        public string SensorTypeName { get; set; }  // Tên loại sensor
        public int PinCode { get; set; }  // Mã pin của sensor
        public bool Status { get; set; }  // Trạng thái của sensor
        public string SensorTypeDescription { get; set; }  // Mô tả loại sensor
        public string SensorTypeUnit { get; set; }  // Đơn vị đo lường của sensor
        public string SensorTypeFieldName { get; set; }  // Tên trường dữ liệu của sensor
    }

}
