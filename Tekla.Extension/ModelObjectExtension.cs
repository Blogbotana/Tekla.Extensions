using System.ComponentModel;
using System.Text.RegularExpressions;
using Tekla.Structures.Model;

namespace Tekla.Extension
{
    /// <summary>
    /// Class for working with <see cref="ModelObject"/> in Tekla Structures 
    /// </summary>
    public static class ModelObjectExtension
    {
        /// <summary>
        /// Get universal report property without sensetive case
        /// </summary>
        /// <typeparam name="T">String, Int, Double</typeparam>
        /// <param name="modelObject">Model object to get property</param>
        /// <param name="name">Name of the attribute</param>
        /// <param name="isSuccess">Is found in Tekla Structures</param>
        /// <returns>result from database</returns>
        public static T GetReportProperty<T>(this ModelObject modelObject, string name, out bool isSuccess)
        {
            var converter = TypeDescriptor.GetConverter(typeof(T));
            string correctedName = Regex.Replace(name.ToUpper().Trim(), @"\s{2,}", " ").Replace(" ", "_");
            if (typeof(T) == typeof(string))
            {
                string value = string.Empty;
                isSuccess = modelObject.GetReportProperty(correctedName, ref value);
                return (T)converter.ConvertTo(value, typeof(T));
            }
            else if (typeof(T) == typeof(int))
            {
                int value = int.MinValue;
                isSuccess = modelObject.GetReportProperty(correctedName, ref value);
                return (T)converter.ConvertTo(value, typeof(T));
            }
            else if (typeof(T) == typeof(double))
            {
                double value = double.MinValue;
                isSuccess = modelObject.GetReportProperty(correctedName, ref value);
                return (T)converter.ConvertTo(value, typeof(T));
            }
            else
            {
                isSuccess = false;
                return default;
            }
        }
        /// <summary>
        /// Get universal report property without sensetive case
        /// </summary>
        /// <typeparam name="T">String, Int, Double</typeparam>
        /// <param name="modelObject">Model object to get property</param>
        /// <param name="name">Name of the attribute</param>
        /// <returns>result from database</returns>
        public static T GetReportProperty<T>(this ModelObject modelObject, string name)
        {
            return GetReportProperty<T>(modelObject, name, out _);
        }
    }
}
