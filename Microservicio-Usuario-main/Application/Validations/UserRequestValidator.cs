using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos.Request;
using FluentValidation;

namespace Application.Validations
{
    public class UserRequestValidator : AbstractValidator<UserRequest>
    {
       public UserRequestValidator() 
        {
            //Validaciones
            RuleFor(u => u.Name)
                .NotEmpty().WithMessage("The User name is required.")
                .MinimumLength(1).WithMessage("The User name must be at least 1 character.");

            RuleFor(u => u.Email)
                .NotEmpty().WithMessage("The Email is required.")
                .EmailAddress().WithMessage("The Email is invalid.");

            RuleFor(u => u.Password)
                .NotEmpty().WithMessage("The password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters");
        }
    }
}
