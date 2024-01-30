namespace PayrollManagementSystem.Business.Dtos.Response
{
    public class WorkHourAdjustmentRes
    {
        public long Id { get; set; }
        public long WorkingHoursLogId { get; set; }
        public long AdjustmentId { get; set; }
        public long SalaryPaymentId { get; set; }
        public decimal AdjustmentAmount { get; set; }
        public decimal AdjustmentPercentage { get; set; }

    }
}