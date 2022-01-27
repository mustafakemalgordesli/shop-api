using FluentValidation;
using shop_api.Models;

namespace shop_api.Validation
{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(product => product.ProductName).NotNull().NotEmpty().WithMessage("product name cannot be empty");
            RuleFor(product => product.CategoryId).NotNull().NotEmpty().WithMessage("categoryıd cannot be empty"); ;
            RuleFor(product => product.UnitPrice).GreaterThan(0).WithMessage("unit price must be greater than 0");
            RuleFor(product => product.UnitPrice).NotNull().NotEmpty().WithMessage("unit price cannot be empty");
            RuleFor(product => product.UnitInStock).NotNull().WithMessage("unit in stock cannot be empty");
        }
    }
}
