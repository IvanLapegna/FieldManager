using Application.Dtos.Request;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validations
{
    public class SignOutRequestValidator : AbstractValidator<SignOutRequest>
    {
        public SignOutRequestValidator() 
        {
            //Validaciones
            RuleFor(s => s.RefreshToken)
                .NotEmpty().WithMessage("The Refresh Token is required.");
        }
    }
}
