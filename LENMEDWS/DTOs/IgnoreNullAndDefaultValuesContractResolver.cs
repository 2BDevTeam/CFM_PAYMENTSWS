using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Reflection;

namespace LENMEDWS.DTOs
{
    public class IgnoreNullAndDefaultValuesContractResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);

            Predicate<object> shouldSerialize = obj =>
            {
                var value = ((PropertyInfo)member).GetValue(obj);
                if (value is decimal && (decimal)value == -1) return false;
                if (value is string && (string)value == "UNSET") return false;
                return true;
            };

            property.ShouldSerialize = shouldSerialize;

            return property;
        }
    }
}
