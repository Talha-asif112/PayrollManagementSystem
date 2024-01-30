using PayrollManagementSystem.Entities.Base;

namespace PayrollManagementSystem.Entities
{
    public class Employee: GeneralBase
    {
        public long AppUserId { get; set; }
        public long DepartmentId { get; set; }
        public AppUser? AppUser { get; set; }
        public DateTime DateOfBirth { get; set; }
        public long JobTitleId { get; set; }
        public JobTitle? JobTitle { get; set; }
        public string? Address { get; set; }
        public DateTime EmploymentStart { get; set; }
    }
}
