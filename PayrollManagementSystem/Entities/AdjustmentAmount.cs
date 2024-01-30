using PayrollManagementSystem.Entities.Base;
using PayrollManagementSystem.Entities;

public class AdjustmentAmount : GeneralBase
{
    public long SalaryPaymentId { get; set; }
    public long AdjustmentId { get; set; }
    public decimal Adjustment_Amount { get; set; }
    public decimal AdjustmentPercentage { get; set; }

    // Navigation properties
    public SalaryPayment SalaryPayment { get; set; }
    public Adjustment Adjustment { get; set; }
}