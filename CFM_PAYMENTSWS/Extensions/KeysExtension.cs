using CFM_PAYMENTSWS.Domains.Models;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Text;

namespace CFM_PAYMENTSWS.Extensions
{
    public static class KeysExtension
    {
        public static string UseThisSizeForStamp(this int size, bool lowerCase = false)
        {
            var builder = new StringBuilder(size);
            // Unicode/ASCII Letters are divided into two blocks
            // (Letters 65–90 / 97–122):
            // The first group containing the uppercase letters and
            // the second group containing the lowercase.  
            // char is a single Unicode character  
            char offset = lowerCase ? 'a' : 'A';
            const int lettersOffset = 26; // A...Z or a..z: length=26  
            for (var i = 0; i < size; i++)
            {
                var @char = (char)new Random().Next(offset, offset + lettersOffset);
                builder.Append(@char);
            }
            return lowerCase ? builder.ToString().ToLower() : builder.ToString();
        }

        public static string AvoidNull(this object obj)
        {
            return obj?.ToString() ?? "";

        }

        public static string NullToEmptyString(object val)
        {
            return val?.ToString() ?? "";
        }
        public static DateTime SafeParseDateTime(this DateTime? value)
        {
            if (DateTime.TryParse(value?.ToString(), out DateTime parsedDate) &&
                parsedDate >= SqlDateTime.MinValue.Value && parsedDate <= SqlDateTime.MaxValue.Value)
            {
                return parsedDate;
            }
            else
            {
                return new DateTime(1900, 1, 1); // Data padrão para casos inválidos ou nulos
            }
        }
        static public string GetFirstTwoCharacters(this string value)
        {
            if (value == null)
            {
                return "";
            }

            return value.Length > 2 ? value.Substring(0, 2) : value;

        }
        static public object GetValObjDy(this object obj, string propertyName)
        {
            return obj.GetType().GetProperty(propertyName).GetValue(obj, null);
        }


        static public string FirstLetterToUpper(this string str)
        {
            if (str == null)
                return null;

            if (str.Length > 1)
                return char.ToUpper(str[0]) + str.Substring(1);

            return str.ToUpper();
        }


        public static string generateRequestId()
        {
            Guid uuid = Guid.NewGuid();
            string uuidAsString = uuid.ToString();

            return "OPMT" + uuidAsString.Substring(0, 21);



        }

        static public string ProcessDescription(this string value)
        {
            Debug.Print($"ProcessDescription {value}");
            if (string.IsNullOrEmpty(value))
            {
                return "N.E.";
            }
            
            string trimmedDescription = value.Trim().ToUpper();

            if (trimmedDescription.Contains("EMPTY") || trimmedDescription.Contains("VAZIO") || trimmedDescription.Contains("N/A"))
            {
                return "N/A";
            }
            else if (trimmedDescription.Contains("PRODUTOS") || trimmedDescription.Contains("PRODUCTOS"))
            {
                return "Produtos Agrícolas";
            }
            else if (trimmedDescription == "N.E." || trimmedDescription == "N.E" || trimmedDescription == "NE")
            {
                return "N.E.";
            }
            else
            {
                // Função para capitalizar a primeira letra de uma string
                return char.ToUpper(trimmedDescription[0]) + trimmedDescription.Substring(1).ToLower();
            }
        }
    }
}
