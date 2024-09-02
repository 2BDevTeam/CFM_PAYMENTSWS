using System.Linq.Expressions;
using System.Reflection;

namespace CFM_PAYMENTSWS.Helper
{
    public class InitializedObject<T>
    {
        public T Object { get; private set; }
        public HashSet<string> AssignedProperties { get; private set; }

        public InitializedObject(T obj, HashSet<string> assignedProperties)
        {
            Object = obj;
            AssignedProperties = assignedProperties;
        }
    }

    public static class ObjectInitializer
    {
        public static InitializedObject<T> Initialize<T>(params Expression<Action<T>>[] initializers) where T : new()
        {
            var assignedProperties = new HashSet<string>();
            var obj = new T();

            foreach (var initializer in initializers)
            {
                if (initializer.Body is MemberExpression memberExpr && memberExpr.Member is PropertyInfo property)
                {
                    assignedProperties.Add(property.Name);
                }

                // Execute the initializer
                var action = initializer.Compile();
                action(obj);
            }

            return new InitializedObject<T>(obj, assignedProperties);
        }
    }
}
