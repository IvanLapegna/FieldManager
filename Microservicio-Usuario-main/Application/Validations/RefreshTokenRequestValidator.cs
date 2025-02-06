using Application.Dtos.Request;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validations
{
    public class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequest>
    {
        public RefreshTokenRequestValidator() 
        {
            //Validaciones
            RuleFor(r => r.ExpiredAccessToken)
                .NotEmpty().WithMessage("The Access Token is required.");

            RuleFor(r => r.RefreshToken)
                .NotEmpty().WithMessage("The Refresh Token is required.");
        }
    }
}
