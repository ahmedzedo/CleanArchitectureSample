using System.Reflection;
using System.Text;

namespace Common.Reflection
{
    public static class ObjectReader
    {
        public static string GetValueForCompainedFields(string[] fullNameFieldsArray, object x)
        {
            StringBuilder result = new();

            foreach (var filed in fullNameFieldsArray)
            {
                var value = x.GetType().GetProperty(filed.Trim())?.GetValue(x, null) as string;

                if (!string.IsNullOrEmpty(value))
                {
                    result.Append(value);
                    result.Append(' ');
                }
            }

            return result.ToString();
        }

        public static void SetPropertyValue<T, TValue>(this T instance, string propertyName, TValue value, BindingFlags bindingFlags = (BindingFlags)16) where T : class
        {
            var idField = typeof(T).GetField($"<{propertyName}>k__BackingField", bindingFlags);
            idField?.SetValue(instance, value);
        }
    }
}
