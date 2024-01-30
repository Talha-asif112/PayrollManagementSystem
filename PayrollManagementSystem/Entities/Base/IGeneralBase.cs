using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TimeWise.Domain.Interfaces;

namespace PayrollManagementSystem.Entities.Base
{
    public interface IGeneralBase : IMinBase
    {
        public bool IsDelete { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public long? ModifiedById { get; set; }
    }
}
