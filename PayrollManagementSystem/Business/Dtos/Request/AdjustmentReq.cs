using System.ComponentModel.DataAnnotations;

namespace PayrollManagementSystem.Business.Dtos.Request
{
    public class AdjustmentReq
    {
        public long Id { get; set; }
        public string AdjustmentName { get; set; }
        [Range(0, 100)]
        public decimal AdjustmentPercentage { get; set; }
        public bool IsWorkingHoursAdjustment { get; set; }
        public bool IsOtherAdjustment { get; set; }
    }
}