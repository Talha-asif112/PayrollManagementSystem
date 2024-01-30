using PayrollManagementSystem.Entities.Base;
using PayrollManagementSystem.Entities;

public class EmploymentTerm : GeneralBase
{
    public long EmployeeId { get; set; }
    public decimal AgreedSalary { get; set; }
    public DateTime SalaryStartDate { get; set; }
    public DateTime? SalaryEndDate { get; set; }
    public Employee Employee { get; set; }
}
