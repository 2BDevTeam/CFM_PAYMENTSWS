using System.Reflection;

namespace CFM_PAYMENTSWS.Extensions
{
    public static class ExtensionMethods
    {
        public static List<string> GetAssignedProperties(this object obj)
        {
            var assignedProperties = new List<string>();
            PropertyInfo[] properties = obj.GetType().GetProperties();

            foreach (var property in properties)
            {
                if (property.PropertyType == typeof(string))
                {
                    string value = property.GetValue(obj) as string;
                    if (value != null)
                    {
                        assignedProperties.Add(property.Name);
                    }
                }
                else if (Nullable.GetUnderlyingType(property.PropertyType) != null)
                {
                    object value = property.GetValue(obj);
                    if (value != null)
                    {
                        assignedProperties.Add(property.Name);
                    }
                }
            }

            return assignedProperties;
        }

        public static void AssignDefaultValues(this object obj)
        {
            PropertyInfo[] properties = obj.GetType().GetProperties();

            foreach (var property in properties)
            {
                if (property.PropertyType == typeof(string))
                {
                    string value = property.GetValue(obj) as string;
                    if (value == null)
                    {
                        property.SetValue(obj, string.Empty); // Default for string
                    }
                }
                else if (property.PropertyType == typeof(DateTime) || Nullable.GetUnderlyingType(property.PropertyType) == typeof(DateTime))
                {
                    object value = property.GetValue(obj);
                    DateTime minValue = new DateTime(1753, 1, 1);
                    DateTime maxValue = new DateTime(9999, 12, 31);
                    DateTime defaultValue = new DateTime(1900, 1, 1);

                    if (value == null || (DateTime)value < minValue || (DateTime)value > maxValue)
                    {
                        property.SetValue(obj, defaultValue);
                    }
                }
                else if (Nullable.GetUnderlyingType(property.PropertyType) != null)
                {
                    object value = property.GetValue(obj);
                    if (value == null)
                    {
                        Type nonNullableType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                        object defaultValue = Activator.CreateInstance(nonNullableType);
                        property.SetValue(obj, defaultValue);
                    }
                }
            }
        }


        public static void AssignDefaultEntityValues<T>(this T obj) where T : class
        {
            PropertyInfo[] properties = obj.GetType().GetProperties();

            foreach (var property in properties)
            {
                if (property.PropertyType == typeof(string))
                {
                    string value = property.GetValue(obj) as string;
                    if (value == null)
                    {
                        property.SetValue(obj, string.Empty); // Default for string
                    }
                }
                else if (property.PropertyType == typeof(DateTime) || Nullable.GetUnderlyingType(property.PropertyType) == typeof(DateTime))
                {
                    object value = property.GetValue(obj);
                    DateTime minValue = new DateTime(1753, 1, 1);
                    DateTime maxValue = new DateTime(9999, 12, 31);
                    DateTime defaultValue = new DateTime(1900, 1, 1);

                    if (value == null || (DateTime)value < minValue || (DateTime)value > maxValue)
                    {
                        property.SetValue(obj, defaultValue);
                    }
                }
                else if (Nullable.GetUnderlyingType(property.PropertyType) != null)
                {
                    object value = property.GetValue(obj);
                    if (value == null)
                    {
                        Type nonNullableType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                        object defaultValue = Activator.CreateInstance(nonNullableType);
                        property.SetValue(obj, defaultValue);
                    }
                }
            }
        }
    }
}
