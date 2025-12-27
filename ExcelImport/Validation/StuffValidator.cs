using ExcelImport.Models;
using FluentValidation;

namespace ExcelImport.Validation
{
    public class StuffValidator : AbstractValidator<Stuff>
    {
        public StuffValidator()
        {
            RuleFor(x => x.BillSerial)
                .InclusiveBetween(1, 9999999999);

            RuleFor(x => x.StuffId)
                .NotEmpty()
                .Matches(@"^\d{13}$");

            RuleFor(x => x.Count)
                .PrecisionScale(18, 2, false)
                .GreaterThan(0);

            RuleFor(x => x.UnitPrice)
                .PrecisionScale(14, 4, false)
                .GreaterThan(0);

            RuleFor(x => x.Discount)
                .PrecisionScale(14, 4, false)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.ValueIncreasedTaxRate)
                .PrecisionScale(4, 2, false)
                .GreaterThanOrEqualTo(0);
        }
    }


}
