
using MicroSoftContract.Exceptions;
using MicroSoftContract.Extensions;
using MicroSoftContract.Infrastructure;
using System.Text.RegularExpressions;

namespace MicroSoftContract.DataModels
{
    public class WorkerDataModel(string id, string fio, string postId, DateTime birthDate, DateTime employmentDate, string email, bool isDeleted) : IValidation
    {
        public string Id { get; private set; } = id;

        public string FIO { get; private set; } = fio;

        public string PostId { get; private set; } = postId;

        public DateTime BirthDate { get; private set; } = birthDate;

        public DateTime EmploymentDate { get; private set; } = employmentDate;

        public string Email { get; private set; } = email;

        public bool IsDeleted { get; private set; } = isDeleted;

        public void Validate()
        {
            if (Id.IsEmpty())
                throw new ValidationException("Field Id is empty");

            if (!Id.IsGuid())
                throw new ValidationException("The value in the field Id is not a unique identifier"); 
      

            if (FIO.IsEmpty())
                throw new ValidationException("Field FIO is empty");

            if (PostId.IsEmpty())
                throw new ValidationException("Field PostId is empty");

            if (!PostId.IsGuid())
                throw new ValidationException("The value in the field PostId is not a unique identifier"); 
      

            if (BirthDate.Date > DateTime.Now.AddYears(-16).Date)
                throw new ValidationException($"Minors cannot be hired (BirthDate = { BirthDate.ToShortDateString() })"); 
      

            if (EmploymentDate.Date < BirthDate.Date)
                throw new ValidationException("The date of employment cannot be less than the date of birth"); 
      

            if ((EmploymentDate - BirthDate).TotalDays / 365 < 16) // EmploymentDate.Year - BirthDate.Year
                throw new ValidationException($"Minors cannot be hired (EmploymentDate - { EmploymentDate.ToShortDateString() }, BirthDate - { BirthDate.ToShortDateString()})");

            if (Email.IsEmpty())
                throw new ValidationException("Field Email is empty");

            string pattern = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*@((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))\z";

            if (!Regex.IsMatch(Email, pattern))
                throw new ValidationException("Field Email is not an e-mail");
        }
    }
}
