using System;
using System.Collections.Generic;
using System.Reflection;

namespace LENMEDWS.Helper
{
    public class ChangeTracker
    {
        private Dictionary<string, object> originalValues = new Dictionary<string, object>();

        public ChangeTracker(object obj)
        {
            foreach (PropertyInfo prop in obj.GetType().GetProperties())
            {
                if (prop.CanRead)
                {
                    originalValues[prop.Name] = prop.GetValue(obj);
                }
            }
        }

        public int? CountChangedFields(object obj)
        {
            int count = 0;
            foreach (PropertyInfo prop in obj.GetType().GetProperties())
            {
                if (prop.CanRead && originalValues.ContainsKey(prop.Name))
                {
                    object originalValue = originalValues[prop.Name];
                    object currentValue = prop.GetValue(obj);

                    // Adicione um ponto de interrupção ou log aqui para verificar os valores
                    Console.WriteLine($"Propriedade: {prop.Name}, Valor Original: {originalValue}, Valor Atual: {currentValue}");

                    if (!Equals(originalValue, currentValue))
                    {
                        count++;
                    }
                }
            }
            return count;
        }
    }

}
