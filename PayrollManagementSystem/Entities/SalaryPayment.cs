using PayrollManagementSystem.Entities.Base;
using PayrollManagementSystem.Entities;

public class SalaryPayment : GeneralBase
{
    public long EmployeeId { get; set; }
    public decimal GrossSalary { get; set; }
    public decimal NetSalary { get; set; }
    public string SalaryPeriod { get; set; }
    public Employee Employee { get; set; }
}
