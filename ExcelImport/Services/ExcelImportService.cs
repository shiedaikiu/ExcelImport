using ExcelDataReader;
using ExcelImport.Data;
using ExcelImport.Models;
using FluentValidation;
using System.Data;
using System.Globalization;
using System.Text;

namespace ExcelImport.Services
{
    public class ExcelImportService
    {
        private readonly AppDbContext _context;
        private readonly IValidator<Bill> _validator;

        public static DateTime PersianToDate(string persianDate)
        {
            var parts = persianDate.Split('/');
            var pc = new PersianCalendar();

            return pc.ToDateTime(
                int.Parse(parts[0]),
                int.Parse(parts[1]),
                int.Parse(parts[2]),
                0, 0, 0, 0);
        }
        public ExcelImportService(AppDbContext context, IValidator<Bill> validator)
        {
            _context = context;
            _validator = validator;
        }


        public async Task ImportAsync(Stream stream)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            using var reader = ExcelReaderFactory.CreateReader(stream);

            var dataSet = reader.AsDataSet();

            var invoiceTable = dataSet.Tables["صورتحساب"];
            var itemTable = dataSet.Tables["اقلام صورتحساب"];

            if (invoiceTable == null || itemTable == null)
                throw new Exception("ساختار فایل اکسل معتبر نمی‌باشد.");

            if (invoiceTable.Rows.Count <= 1 && itemTable.Rows.Count <= 1)
                throw new Exception("فایل اکسل خالی است و داده‌ای برای ثبت وجود ندارد.");
            var invoices = new List<Bill>();
            var items = new List<Stuff>();

            for (int i = 1; i < invoiceTable.Rows.Count; i++)
            {
                var row = invoiceTable.Rows[i];

                var invoice = new Bill
                {
                    BillSerial = Convert.ToInt64(row[0].ToString()),
                    CreateDate = Convert.ToDateTime(row[1].ToString()),
                    BuyerType = (BuyerTypeEnum)Convert.ToInt32(row[2]),
                    EconomicCode = row[3].ToString(),
                    TotalPrice = Convert.ToDecimal(row[4])
                };

                invoices.Add(invoice);
            }

            for (int i = 1; i < itemTable.Rows.Count; i++)
            {
                var row = itemTable.Rows[i];

                var item = new Stuff
                {
                    BillSerial = Convert.ToInt64(row[0].ToString()),
                    StuffId = row[1].ToString(),
                    Count = Convert.ToDecimal(row[2]),
                    UnitPrice = Convert.ToDecimal(row[3]),
                    Discount = Convert.ToDecimal(row[4]),
                    ValueIncreasedTaxRate = Convert.ToInt32(row[5])
                };

                items.Add(item);
            }

            _context.Bills.AddRange(invoices);
            _context.Stuffs.AddRange(items);
            await _context.SaveChangesAsync();
        }

    }

}
