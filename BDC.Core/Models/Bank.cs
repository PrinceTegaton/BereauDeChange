namespace BDC.Core.Models
{
    public class Bank : BaseModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public bool IsLocal { get; set; }
    }
}
