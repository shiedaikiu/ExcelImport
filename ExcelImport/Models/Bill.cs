using System.ComponentModel;

namespace ExcelImport.Models
{
    public class Bill
    {
        public int Id { get; set; }

        public long BillSerial { get; set; }

        public DateTime CreateDate { get; set; }

        public BuyerTypeEnum BuyerType { get; set; }

        public string EconomicCode { get; set; }

        public decimal TotalPrice { get; set; }

        public ICollection<Stuff> Items { get; set; } = new List<Stuff>();
    }
    public enum BuyerTypeEnum
    {
        [Description("حقیقی")]
        Real = 1,
        [Description("حقوقی")]
        Legal = 2,
        [Description("اتباع خارجی")]
        Foriegners = 3
    }

}
