namespace PayrollManagementSystem.Business.Dtos.Response
{
    public class EmployeeRes
    {
        public long Id { get; set; }
        public DateTime DateOfBirth { get; set; }
        public long DepartmentId { get; set; }
        public long JobTitleId { get; set; }
        public string? Address { get; set; }
        public DateTime EmploymentStart { get; set; }
    }
}
