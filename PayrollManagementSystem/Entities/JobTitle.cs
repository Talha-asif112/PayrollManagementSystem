using PayrollManagementSystem.Entities.Base;
using PayrollManagementSystem.Entities;

public class JobTitle : GeneralBase
{
    public string Title { get; set; }
    public IList<Employee> Employees { get; set; }
}
