using FluentValidation;
using shop_api.Models;

namespace shop_api.Validation
{
    public class OrderValidator : AbstractValidator<Order>
    {
        public OrderValidator()
        {
            RuleFor(order => order.ProductId).NotNull().NotEmpty().WithMessage("product ıd cannot be empty");
            RuleFor(order => order.Quantity).GreaterThan(0).WithMessage("quantity must be greater than 0");
            RuleFor(order => order.Quantity).NotNull().NotEmpty().WithMessage("quantity cannot be empty");
        }
    }
}
