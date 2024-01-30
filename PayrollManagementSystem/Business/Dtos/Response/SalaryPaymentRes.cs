namespace PayrollManagementSystem.Business.Dtos.Response
{
    public class SalaryPaymentRes
    {
        public long EmployeeId { get; set; }
        public decimal GrossSalary { get; set; }
        public decimal NetSalary { get; set; }
        public string? SalaryPeriod { get; set; }
    }
}