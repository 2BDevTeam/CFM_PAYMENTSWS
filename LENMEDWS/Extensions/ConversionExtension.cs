using System.Data.SqlTypes;
using System.Diagnostics;
using System.Globalization;

namespace LENMEDWS.Extensions
{
    public class ConversionExtension
    {

        public decimal? toDecimal(string value)
        {
            try { return Convert.ToDecimal(value); }

            catch (Exception e) { return 000; }

        }

        public bool? toBool(string value)
        {

            try { return (value == "1" || Int32.Parse(value) == 1 ? true : false); }
            catch (Exception e) { return false; }
        }
        public DateTime? ParseToDate(object value)
        {
            if (DateTime.TryParse(value?.ToString(), out DateTime parsedDate) &&
                parsedDate >= SqlDateTime.MinValue.Value && parsedDate <= SqlDateTime.MaxValue.Value)
            {
                return parsedDate;
            }
            else
            {
                return new DateTime(1900, 1, 1);
            }
        }

        public string getLastTwoCharacters(string value)
        {
            if (value == null)
            {
                return "";
            }
           
            return value.Length > 2 ? value.Substring(value.Length - 2) : value;
            
        }
        public string nullToString(object value)
        {


            return value == null ? "" : value.ToString().Trim();



        }
        public DateTime? toDateTime(string value, string format)
        {
            try { return DateTime.ParseExact(value, format, CultureInfo.InvariantCulture); }
            catch (Exception e) { return new DateTime(); };
        }

        public decimal? toTons(string value)
        {
            try
            {
                // Debug.Print(decimal.Parse(value).ToString());
                return Math.Round(Decimal.Divide(decimal.Parse(value, CultureInfo.InvariantCulture), 1000), 3);
            }
            catch (Exception e) { return 0; }
        }

    }
}
