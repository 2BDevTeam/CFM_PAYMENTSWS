using Newtonsoft.Json;
using System.Linq.Expressions;

namespace CFM_PAYMENTSWS.Helper
{
    public class TrackChanges<T>
    {
        private T _obj;
        private HashSet<string> _changedProperties = new HashSet<string>();

        public TrackChanges(T obj)
        {
            _obj = obj;
        }

        public T Obj => _obj;

        public void SetProperty<TValue>(Expression<Func<T, TValue>> selector, TValue value)
        {
            var memberExpression = (MemberExpression)selector.Body;
            var propertyName = memberExpression.Member.Name;

            typeof(T).GetProperty(propertyName).SetValue(_obj, value);
            _changedProperties.Add(propertyName);
        }

        public TValue GetProperty<TValue>(Expression<Func<T, TValue>> selector)
        {
            var memberExpression = (MemberExpression)selector.Body;
            var propertyName = memberExpression.Member.Name;

            return (TValue)typeof(T).GetProperty(propertyName).GetValue(_obj);
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(_obj, Formatting.Indented);
        }

        public int? GetChangedPropertiesCount()
        {
            return _changedProperties.Count;
        }

        public List<string> GetChangedProperties()
        {
            return _changedProperties.ToList();
        }
    }
}
