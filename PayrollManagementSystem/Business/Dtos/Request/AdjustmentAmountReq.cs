namespace PayrollManagementSystem.Business.Dtos.Request
{
    public class AdjustmentAmountReq
    {
        public long SalaryPaymentId { get; set; }
        public long AdjustmentId { get; set; }
        public decimal Adjustment_Amount { get; set; }
        public decimal AdjustmentPercentage { get; set; }
    }
}