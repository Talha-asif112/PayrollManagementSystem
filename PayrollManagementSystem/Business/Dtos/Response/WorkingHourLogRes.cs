namespace PayrollManagementSystem.Business.Dtos.Response
{
    public class WorkingHourLogRes
    {
        public long EmployeeId { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? TimeIn { get; set; }
        public DateTime? TimeOut { get; set; }
        public TimeSpan? WorkDuration { get; set; }
    }
}
