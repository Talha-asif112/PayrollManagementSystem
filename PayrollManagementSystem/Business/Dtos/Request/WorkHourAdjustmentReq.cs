namespace PayrollManagementSystem.Business.Dtos.Request
{
    public class WorkHourAdjustmentReq
    {
        public long WorkingHoursLogId { get; set; }
        public long AdjustmentId { get; set; }
        public long SalaryPaymentId { get; set; }
        public decimal AdjustmentAmount { get; set; }
        public decimal AdjustmentPercentage { get; set; }

    }
}