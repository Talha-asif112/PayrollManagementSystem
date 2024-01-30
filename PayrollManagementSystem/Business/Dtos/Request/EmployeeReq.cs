using System.Runtime.CompilerServices;

namespace PayrollManagementSystem.Business.Dtos.Request
{
    public class EmployeeReq
    {
        public long Id { get; set; }

        public long AppUserId { get; set; }
        public DateTime DateOfBirth { get; set; }
        public long DepartmentId { get; set; }
        public long JobTitleId { get; set; }
        public string? Address { get; set; }
        public DateTime EmploymentStart { get; set; }
    }
}
