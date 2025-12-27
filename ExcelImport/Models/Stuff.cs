namespace ExcelImport.Models
{
    public class Stuff
    {
        public int Id { get; set; }

        public long BillSerial { get; set; }

        public string StuffId { get; set; }

        public decimal Count { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal Discount { get; set; }

        public decimal ValueIncreasedTaxRate { get; set; }

        public Bill Bill { get; set; }
    }

}
