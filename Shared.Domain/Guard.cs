using System;

namespace Shared.Domain
{
    public class Guard
    {
        public static void ForNull(object value, string parameterName)
        {
            if (value == null)
            {
                throw new ArgumentOutOfRangeException(parameterName);
            }
        }
        public static void ForNull(object value, string parameterName, string message)
        {
            if (value == null)
            {
                throw new ArgumentOutOfRangeException(parameterName, message);
            }
        }

        public static void ForLessEqualZero(int value, string parameterName)
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(parameterName);
            }
        }


        public static void ForLessEqualZero(decimal value, string parameterName)
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(parameterName);
            }
        }

        public static void ForLessEqualZero(double value, string parameterName)
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(parameterName);
            }
        }

        public static void ForPrecedesDate(DateTime value, DateTime dateToPrecede, string parameterName)
        {
            if (value >= dateToPrecede)
            {
                throw new ArgumentOutOfRangeException(parameterName);
            }
        }

        public static void ForNullOrEmpty(string value, string parameterName)
        {
            if (String.IsNullOrEmpty(value))
            {
                throw new ArgumentOutOfRangeException(parameterName);
            }
        }


        public static void ForNullOrEmpty(Guid value, string parameterName)
        {
            if (value == null || value == Guid.Empty)
            {
                throw new ArgumentOutOfRangeException(parameterName);
            }
        }
    }

}