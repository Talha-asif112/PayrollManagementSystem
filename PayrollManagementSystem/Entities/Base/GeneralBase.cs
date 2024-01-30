namespace PayrollManagementSystem.Entities.Base
{
    public class GeneralBase : IGeneralBase
    {
        public long Id { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
        public long? CreatedById { get; set; }
        public bool IsDelete { get; set; } = false;
        public DateTime? ModifiedDate { get; set; }
        public long? ModifiedById { get; set; }
    }
}

