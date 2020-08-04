using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KairosTest.Helpers
{
    public class RequiredPositiveValue : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            try
            {
                decimal i = Convert.ToDecimal(value.ToString());
                if (i < 0)
                    return false;
                else
                    return true;
            }
            catch (Exception)
            {
                return false;
            }
            // return true if value is a non-null number >= 0, otherwise return false

        }
    }

    public class RequiredGreaterThanZero : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            try
            {
                decimal i = Convert.ToDecimal(value.ToString());
                if (i <= 0)
                    return false;
                else
                    return true;
            }
            catch (Exception)
            {
                return false;
            }
            // return true if value is a non-null number >= 0, otherwise return false

        }
    }
}
