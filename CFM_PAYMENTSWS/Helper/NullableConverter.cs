using System.Dynamic;
using System.Reflection;

namespace CFM_PAYMENTSWS.Helper
{
    public static class NullableConverter
    {

        public static T ChangeType<T>(object value)
        {
            var t = typeof(T);

            if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                {
                    return default(T);
                }

                t = Nullable.GetUnderlyingType(t);
            }

            return (T)Convert.ChangeType(value, t);
        }

        public static T To<T>(this IConvertible obj)
        {
            Type t = typeof(T);
            Type u = Nullable.GetUnderlyingType(t);

            if (u != null)
            {
                return (obj == null) ? default(T) : (T)Convert.ChangeType(obj, u);
            }
            else
            {
                return (T)Convert.ChangeType(obj, t);
            }
        }


        public static object ConvertToNullable<T>(T obj)
        {
            Type type = typeof(T);
            var nullableType = typeof(Nullable<>);

            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj), "Input object cannot be null.");
            }

            var newObj = Activator.CreateInstance(type);
            if (newObj == null)
            {
                throw new InvalidOperationException($"Could not create an instance of type {type.FullName}.");
            }

            foreach (var property in type.GetProperties())
            {
                if (!property.CanRead || !property.CanWrite) continue;

                Type propType = property.PropertyType;
                Type newPropType = propType.IsValueType ? nullableType.MakeGenericType(propType) : propType;

                var value = property.GetValue(obj);
                var changeTypeMethod = typeof(Program).GetMethod(nameof(ChangeType));
                if (changeTypeMethod == null)
                {
                    throw new InvalidOperationException("ChangeType method not found.");
                }

                var genericChangeTypeMethod = changeTypeMethod.MakeGenericMethod(newPropType);
                if (genericChangeTypeMethod == null)
                {
                    throw new InvalidOperationException("Failed to make a generic version of ChangeType method.");
                }

                var newValue = genericChangeTypeMethod.Invoke(null, new object[] { value });
                property.SetValue(newObj, newValue);
            }

            return newObj;
        }

    }
}
