using System.ComponentModel.DataAnnotations;

namespace CSWeb1PE.ModelValidation
{
    public class MaximumDateCurrentYearAttribute : ValidationAttribute
    {
        private readonly int _maximumMonth;
        private readonly int _maximumDay;

        public MaximumDateCurrentYearAttribute(int maximumMonth, int maximumDay)
        {
            _maximumMonth = maximumMonth;
            _maximumDay = maximumDay;
        }

        public override bool IsValid(object? value)
        {
            if (value == null)
            {
                return true;
            }

            DateTime dateValue = (DateTime)value;
            DateTime maximumDate = new DateTime(DateTime.Now.Year, _maximumMonth, _maximumDay);
            return dateValue <= maximumDate;
        }
    }
}
