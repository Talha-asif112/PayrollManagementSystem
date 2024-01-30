using PayrollManagementSystem.Entities.Base;
using PayrollManagementSystem.Entities;

public class Department : GeneralBase
{
    public string DepartmentName { get; set; }
    public IList<Employee> Employees { get; set; }
}
