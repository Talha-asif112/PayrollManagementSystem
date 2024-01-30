namespace PayrollManagementSystem.Business.Dtos.Request
{
    public class SalaryPaymentReq
    {
        public long EmployeeId { get; set; }
        public decimal GrossSalary { get; set; }
        public decimal NetSalary { get; set; }
        public string? SalaryPeriod { get; set; }
    }
}