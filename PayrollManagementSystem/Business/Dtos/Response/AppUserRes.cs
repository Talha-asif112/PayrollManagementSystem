namespace PayrollManagementSystem.Business.Dtos.Response
{
    public class AppUserRes
    {
        public long? Id { get; set; }
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public string FirstName { get; set; } = null!;
        public string Role { get; set; } = null!; // This is Req
        public string? LastName { get; set; }
        public string? ContactNo { get; set; }
    }
}
