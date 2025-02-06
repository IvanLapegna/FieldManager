using Application.Dtos.Request;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validations
{
    public class PasswordChangeRequestValidator : AbstractValidator<PasswordChangeRequest>
    {
        public PasswordChangeRequestValidator() 
        {
            //Validaciones
            RuleFor(p => p.CurrentPassword)
                .NotEmpty().WithMessage("The current password is required.");

            RuleFor(p => p.NewPassword)
                .NotEmpty().WithMessage("The new password is required.")
                .MinimumLength(8).WithMessage("The new Password must be at least 8 characters")
                .Must((request, newPassword) => newPassword != request.CurrentPassword)
                .WithMessage("The new password cannot be the same as the current password.");
        }
    }
}
