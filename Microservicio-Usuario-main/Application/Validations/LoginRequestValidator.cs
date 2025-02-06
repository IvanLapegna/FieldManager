using Application.Dtos.Request;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validations
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator() 
        {
            //Validaciones
            RuleFor(l => l.Email)
                .NotEmpty().WithMessage("The Email is required.")
                .EmailAddress().WithMessage("The Email is invalid.");

            RuleFor(l => l.Password)
                .NotEmpty().WithMessage("The password is required.");
        }
    }
}
