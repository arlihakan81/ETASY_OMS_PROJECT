using ETASY_OMS_PROJECT.WebUI.Entity.Entities.Concrete;

namespace ETASY_OMS_PROJECT.WebUI.Entity.Entities
{
    public class Group : EntityBase
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsExpense { get; set; }
        public bool IsMonthly { get; set; }
        public byte Quantity { get; set; }
        public decimal Amount { get; set; }

    }
}
