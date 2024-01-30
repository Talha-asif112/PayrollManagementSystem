using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

using PayrollManagementSystem.Entities.Base;


namespace PayrollManagementSystem.Entities
{
    public class AppUser : IdentityUser<long>, IGeneralBase
    {
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public string Role { get; set; }
        public string ContactNo { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDelete { get; set; } = false;
        public DateTime? ModifiedDate { get; set; }
        public long? CreatedById { get; set; }
        public long? ModifiedById { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";

    }
}
