using System;

namespace BDC.Core.Models
{
    public class BaseModel
    {
        public Guid Id { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public DateTime ModifiedOn { get; set; }
        public bool IsDeleted { get; set; }
    }
}
