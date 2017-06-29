using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organizer.Common.Helpers
{
    public static class EnumExtentions
    {
        private static string AddSpacesBetweenNumbersAndRestOfString(string source)
        {
            var result = new StringBuilder();
            int index = 0, endIndex = 0;
            bool digitsSymbolsFlag = false;

            int counter = 0;
            foreach (var value in source)
            {
                if (char.IsDigit(value))
                {
                    if (!digitsSymbolsFlag)
                    {
                        digitsSymbolsFlag = true;
                        index = counter;
                    }
                    endIndex = counter;
                }

                if (char.IsLetter(value) && digitsSymbolsFlag)
                {
                    digitsSymbolsFlag = false;
                    var digitsFromSource = source.Substring(index, endIndex - index + 1);
                    result.Replace(digitsFromSource, " " + digitsFromSource + "");
                }

                result.Append(value);

                counter++;
            }
            return result.ToString();
        }

        public static string ConvertToString(this Enum enumValue)
        {
            var str = enumValue.GetType().GetEnumName(enumValue);
            if (str.Contains("_"))
            {
                str = str.Replace("_", " ");
            }

            var i = 0;

            if (str.Any(char.IsDigit))
            {
                str = AddSpacesBetweenNumbersAndRestOfString(str);
            }

            var sb = new StringBuilder();
            foreach (var symbol in str)
            {
                if (char.IsUpper(symbol) && i != 0)
                {
                    sb.Append(" " + char.ToLower(symbol));
                }
                else
                {
                    sb.Append(symbol);
                }
                i++;
            }

            return sb.ToString().Trim();
        }

        public static T ConvertToEnum<T>(this string value) where T : struct, IComparable, IFormattable, IConvertible
        {
            if (!typeof(T).IsEnum) { throw new ArgumentException("T must be an enumerated type"); }
            T result = default(T);
            var values = typeof(T).GetEnumValues();

            foreach (var item in values)
            {
                if (((Enum)item).ConvertToString() == value)
                {
                    result = (T)item;
                    break;
                }
            }
            return result;
        }

        public static IEnumerable<T> GetAllValuesOfEnum<T>(this T type) where T : struct, IComparable, IFormattable, IConvertible
        {
            if (!typeof(T).IsEnum) { throw new ArgumentException("T must be an enumerated type"); }
            return typeof(T).GetEnumValues().Cast<T>();
        }
    }
}