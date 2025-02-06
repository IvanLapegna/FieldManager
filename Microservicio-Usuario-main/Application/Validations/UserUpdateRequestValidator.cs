using Application.Dtos.Request;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validations
{
    public class UserUpdateRequestValidator : AbstractValidator<UserUpdateRequest>
    {
        public UserUpdateRequestValidator() 
        {
            //Validaciones
            RuleFor(u => u.Name)
                .MinimumLength(1).WithMessage("The User name must be at least 1 character.")
                .When(u => !string.IsNullOrEmpty(u.Name));

            RuleFor(u => u.Email)
                .EmailAddress().WithMessage("The Email is invalid.")
                .When(u => !string.IsNullOrEmpty (u.Email));
        }
    }
}
