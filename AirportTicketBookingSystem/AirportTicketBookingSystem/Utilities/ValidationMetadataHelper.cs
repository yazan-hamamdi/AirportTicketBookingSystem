using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace AirportTicketBookingSystem.Utilities
{
    public static class ValidationMetadataHelper
    {
        public static List<string> GetPropertyConstraints(PropertyInfo prop)
        {
            var constraints = new List<string>();
            var attributes = prop.GetCustomAttributes(true);

            foreach (var attr in attributes)
            {
                switch (attr)
                {
                    case RequiredAttribute _:
                        constraints.Add("Required");
                        break;

                    case RangeAttribute range:
                        constraints.Add($"Allowed Range ({range.Minimum} → {range.Maximum})");
                        break;

                    case StringLengthAttribute strLen:
                        constraints.Add($"String Length (Max {strLen.MaximumLength})");
                        break;

                    case DataTypeAttribute dataType:
                        constraints.Add($"Data Type: {dataType.DataType}");
                        break;
                }
            }
            return constraints;
        }
    }
}
