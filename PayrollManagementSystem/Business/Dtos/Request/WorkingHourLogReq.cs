namespace PayrollManagementSystem.Business.Dtos.Request
{
    public class WorkingHourLogReq
    {
        public long EmployeeId { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? TimeIn { get; set; }
        public DateTime? TimeOut { get; set; }
        public TimeSpan? WorkDuration { get; set; }
    }
}
