using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MCS.HomeSite.Common
{
    internal static partial class Extensions
    {
        public static void SetSessionData<T>(this HttpContext context, string key, T value)
        {
            try
            {
                if (value is null)
                {
                    context.Session.Remove(key);
                    return;
                }
                var jsonString = JsonConvert.SerializeObject(value);
                context.Session.Set(key, Encoding.UTF8.GetBytes(jsonString));
            }
            catch { }
        }

        public static void SetSessionData(this HttpContext context, string key, string value)
        {
            try
            {
                if (string.IsNullOrEmpty(value))
                {
                    context.Session.Remove(key);
                    return;
                }
                context.Session.Set(key, Encoding.UTF8.GetBytes(value));
            }
            catch { }
        }

        public static bool TryGetSessionData<T>(this HttpContext context, string key, out T sessionValue)
        {
            sessionValue = (T)Activator.CreateInstance(typeof(T));
            try
            {
                if (!context.Session.TryGetValue(key, out var value)) return false;
                var data = Encoding.UTF8.GetString(value);
                if (string.IsNullOrEmpty(data))
                    return false;
                sessionValue = JsonConvert.DeserializeObject<T>(data);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool TryGetSessionData(this HttpContext context, string key, out string sessionValue)
        {
            sessionValue = null;
            try
            {
                if (!context.Session.TryGetValue(key, out var value)) return false;
                sessionValue = Encoding.UTF8.GetString(value);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void ClearSession(this HttpContext context)
        {
            try
            {
                context.Session.Clear();
            }
            catch { }
        }

        public static decimal RoundTwoDecimal(this decimal? value, int decimalPlaces = 2)
        {
            return Math.Round(value ?? 0, decimalPlaces, MidpointRounding.AwayFromZero);
        }

        public static decimal RoundTwoDecimal(this decimal value, int decimalPlaces = 2)
        {
            return Math.Round(value, decimalPlaces, MidpointRounding.AwayFromZero);
        }

        public static string GetDisplayName<T>(this T enumerationValue) where T : struct
        {
            try
            {
                ArgumentNullException.ThrowIfNull(enumerationValue);
                var type = enumerationValue.GetType();
                if (!type.IsEnum)
                {
                    throw new ArgumentException($"{nameof(enumerationValue)} must be of Enum type", nameof(enumerationValue));
                }
                var memberInfo = type.GetMember(enumerationValue.ToString());
                if (memberInfo.Length > 0)
                {
                    var attrs = memberInfo[0].GetCustomAttributes(typeof(DisplayAttribute), false);

                    if (attrs.Length > 0)
                    {
                        return ((DisplayAttribute)attrs[0]).Name;
                    }
                }
                return enumerationValue.ToString();
            }
            catch
            {
                return default;
            }
        }

        public static string GetFullMessage(this Exception ex)
        {
            var sb = new StringBuilder();
            sb.AppendLine(ex.Message);

            if (ex is AggregateException aggregateException)
            {
                foreach (var exception in aggregateException.InnerExceptions)
                    sb.AppendLine(GetFullMessage(exception));
            }
            else
            {
                if (ex.InnerException != null)
                    sb.AppendLine(GetFullMessage(ex.InnerException));
            }
            return sb.ToString().TrimEnd(Environment.NewLine.ToCharArray());
        }

        public static IProperty GetProperty(this IReadOnlyList<IProperty> properties, string propertyName)
        {
            foreach (var property in properties)
            {
                if (property.Name.Equals(propertyName, StringComparison.InvariantCultureIgnoreCase))
                    return property;
            }
            return null;
        }

        public static T GetPropertyValue<T>(this PropertyValues propertyValues, string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
                return default;
            var property = propertyValues.Properties.GetProperty(propertyName);
            if (property == null)
                return default;
            var value = propertyValues[property];
            if (value is null)
                return default;

            var t = typeof(T);

            if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                t = Nullable.GetUnderlyingType(t);

            var propertyValue = propertyValues[property];

            if (propertyValue is DateTime time && typeof(T) == typeof(string))
            {
                propertyValue = time.ToString("O");
            }

            return (T)Convert.ChangeType(propertyValue, t);
        }

        public static bool HasVersionNumber(this string customerPa)
        {
            if (string.IsNullOrEmpty((customerPa)))
                return false;
            var index = customerPa.LastIndexOf("_") + 2;
            var data = customerPa[index..];
            var result = decimal.TryParse(data, out var value);
            return result && value > 0;
        }
    }
}
