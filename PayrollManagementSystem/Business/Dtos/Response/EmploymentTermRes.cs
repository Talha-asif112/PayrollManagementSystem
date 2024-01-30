namespace PayrollManagementSystem.Business.Dtos.Response
{
    public class EmploymentTermRes
    {
        public long Id { get; set; }
        public long EmployeeId { get; set; }
        public decimal AgreedSalary { get; set; }
        public DateTime SalaryStartDate { get; set; }
        public DateTime? SalaryEndDate { get; set; }
    }
}