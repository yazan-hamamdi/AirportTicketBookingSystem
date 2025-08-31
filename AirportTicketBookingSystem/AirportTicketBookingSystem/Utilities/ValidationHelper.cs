using System.ComponentModel.DataAnnotations;

namespace AirportTicketBookingSystem.Utilities
{
    public static class ValidationHelper
    {
        public static List<string> ValidateModel<T>(T model)
        {
            var context = new ValidationContext(model);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(model, context, results, true);

            if (isValid)
                return new List<string>();

            return results
                  .Where(r => r.ErrorMessage != null)
                  .Select(r => r.ErrorMessage!)
                  .ToList();
        }
    }
}