using PayrollManagementSystem.Entities.Base;

public class Adjustment : GeneralBase
{
    public string AdjustmentName { get; set; }
    public decimal AdjustmentPercentage { get; set; }
    public bool IsWorkingHoursAdjustment { get; set; }
    public bool IsOtherAdjustment { get; set; }

    // Navigation property
    public IList<WorkHourAdjustment> WorkHourAdjustments { get; set; }
    public IList<AdjustmentAmount> AdjustmentAmounts { get; set; }
}
