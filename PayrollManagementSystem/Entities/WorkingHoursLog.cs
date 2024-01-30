using PayrollManagementSystem.Entities.Base;

namespace PayrollManagementSystem.Entities
{
    public class WorkingHoursLog : GeneralBase
    {
        public long EmployeeId { get; set; }
        public Employee? Employee { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? TimeIn { get; set; }
        public DateTime? TimeOut { get; set; }
        public TimeSpan? WorkDuration { get; set; }
    }
}
