using System.ComponentModel.DataAnnotations;

namespace CSWeb1PE.ModelValidation
{
    public class MinimumDateAttribute : ValidationAttribute
    {
        private readonly int _minimumYear;
        private readonly int _minimumMonth;
        private readonly int _minimumDay;

        public MinimumDateAttribute(int minimumYear, int minimumMonth, int minimumDay)
        {
            _minimumYear = minimumYear;
            _minimumMonth = minimumMonth;
            _minimumDay = minimumDay;
        }

        public override bool IsValid(object? value)
        {
            if (value == null)
            {
                return true;
            }

            DateTime dateValue = (DateTime)value;
            DateTime minimumDate = new DateTime(_minimumYear, _minimumMonth, _minimumDay);
            return dateValue >= minimumDate;
        }
    }
}
