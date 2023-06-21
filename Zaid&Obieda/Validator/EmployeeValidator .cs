using FluentValidation;
using Zaid_Obieda.Models;

namespace Zaid_Obieda.Validator
{
    public class EmployeeValidator : AbstractValidator<Employee>
    {
        public EmployeeValidator()
        {
            RuleFor(q => q.EmployeeName).NotEmpty().WithMessage("The Employee Name Field is required !!");
            RuleFor(q => q.Age).NotEmpty().WithMessage("The Age Field is required !!");
        }
    }
}
