using FluentValidation;
using shop_api.Models;

namespace shop_api.Validation
{
    public class CategoryValidator : AbstractValidator<Category>
    {
        public CategoryValidator()
        {
            RuleFor(category => category.CategoryName).NotNull().NotEmpty().WithMessage("category name cannot be empty");
        }
    }
}
