using PayrollManagementSystem.Entities.Base;
using PayrollManagementSystem.Entities;

public class WorkHourAdjustment : GeneralBase
{
    public long WorkingHoursLogId { get; set; }
    public long AdjustmentId { get; set; }
    public long SalaryPaymentId { get; set; }
    public decimal AdjustmentAmount { get; set; }
    public decimal AdjustmentPercentage { get; set; }
    public WorkingHoursLog WorkingHoursLog { get; set; }
    public Adjustment Adjustment { get; set; }
    public SalaryPayment SalaryPayment { get; set; }
}
