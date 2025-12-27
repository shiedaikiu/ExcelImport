using ExcelImport.Models;
using FluentValidation;

namespace ExcelImport.Validation
{
    public class BillValidator : AbstractValidator<Bill>
    {
        public BillValidator()
        {
            RuleFor(x => x.BillSerial)
                .InclusiveBetween(1, 9999999999);

            RuleFor(x => x.CreateDate)
                .NotEmpty();

            RuleFor(x => x.BuyerType)
                .IsInEnum();

            RuleFor(x => x.EconomicCode)
                .NotEmpty()
                .MaximumLength(20);

            RuleFor(x => x.TotalPrice)
                .PrecisionScale(18, 2, false)
                .GreaterThanOrEqualTo(0);

            RuleForEach(x => x.Items)
                .SetValidator(new StuffValidator());
        }
    }


}
