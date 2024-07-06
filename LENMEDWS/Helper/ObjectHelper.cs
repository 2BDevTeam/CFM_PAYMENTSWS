using LENMEDWS.DTOs;
using System.Reflection;

namespace LENMEDWS.Helper
{
    public class ObjectHelper
    {

        public  object ConvertToType(Type targetType, object value)
        {
            if (targetType == null)
                throw new ArgumentNullException(nameof(targetType));

            if (value == null || targetType.IsInstanceOfType(value))
                return value;

            try
            {
                return Convert.ChangeType(value, targetType);
            }
            catch (InvalidCastException)
            {
                // Opção: Retornar null ou lançar a exceção para o chamador
                return null;
            }
            catch (FormatException)
            {
                // Opção: Retornar null ou lançar a exceção para o chamador
                return null;
            }
            catch (OverflowException)
            {
                // Opção: Retornar null ou lançar a exceção para o chamador
                return null;
            }
        }

        private PropertyInfo FindModelProperty(Type objType, string modelName)
        {
            foreach (PropertyInfo property in objType.GetProperties())
            {
                if (property.PropertyType.Name.Equals(modelName, StringComparison.OrdinalIgnoreCase))
                {
                    return property;
                }
            }
            return null;
        }
        public PropertyResult ScanObjectValue(object obj, string modelName, string propertyName)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            Type objType = obj.GetType();
            PropertyInfo modelProperty = FindModelProperty(objType, modelName);

            if (modelProperty != null)
            {
                object modelInstance = modelProperty.GetValue(obj);
                if (modelInstance != null)
                {
                    PropertyInfo targetProperty = modelInstance.GetType().GetProperty(propertyName);
                    if (targetProperty != null)
                    {
                        return new PropertyResult
                        {
                            PropertyValue = targetProperty.GetValue(modelInstance),
                            PropertyType = targetProperty.PropertyType
                        };
                    }
                }
            }

            return null; // ou retornar uma instância de PropertyResult vazia
        }

    }

   
}
