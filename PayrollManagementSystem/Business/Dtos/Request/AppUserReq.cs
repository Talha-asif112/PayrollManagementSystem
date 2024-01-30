namespace PayrollManagementSystem.Business.Dtos.Request
{
    public class AppUserReq
    {
        public long? Id { get; set; }
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public string FirstName { get; set; } = null!;
        public string Role { get; set; } = null!;
        public string? LastName { get; set; }
        public string? ContactNo { get; set; }
    }
}
