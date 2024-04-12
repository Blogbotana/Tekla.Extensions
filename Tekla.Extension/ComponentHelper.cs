using System;
using System.Collections.Generic;
using Tekla.Structures;
using Tekla.Structures.Model;

namespace Tekla.Extension;
/// <summary>
/// Helps to work with components.
/// </summary>
public static class ComponentHelper
{
    /// <summary>
    /// Delete all cache objects of the component.
    /// </summary>
    /// <param name="identifier"></param>
    public static void DeleteChildren(Identifier identifier)
    {
        Component component = new()
        {
            Identifier = identifier
        };

        if (!component.Select())
        {
            return;
        }

        foreach (ModelObject child in component.GetChildren().ToIEnumerable<ModelObject>())
        {
            if (child is not null && (child.IsConnectionObject() || child.IsAssociativeObject()))
            {
                _ = child.Delete();
            }
        }
    }
    /// <summary>
    /// Delete all objects of the component.
    /// </summary>
    /// <param name="identifier"></param>
    public static void DeleteAllChildren(Identifier identifier)
    {
        Component component = new()
        {
            Identifier = identifier
        };

        if (!component.Select())
            return;

        foreach (ModelObject child in component.GetChildren().ToIEnumerable<ModelObject>())
        {
            if (child is not null)
                _ = child.Delete();
        }
    }
    public static T GetEnumProperty<T>(this BaseComponent component, string attr, int defaultNumber) where T : Enum
    {
        int number = component.GetUDAProperty<int>(attr, out _);
        if (number < 0)
            number = defaultNumber;

        return (T)Enum.ToObject(typeof(T), number);
    }
    public static double GetDoubleProperty(this BaseComponent component, string attr, double defaultNumber)
    {
        double number = component.GetUDAProperty<double>(attr, out _);
        if (number < 0)
            number = defaultNumber;

        return number;
    }
    public static int GetIntProperty(this BaseComponent component, string attr, int defaultNumber)
    {
        int number = component.GetUDAProperty<int>(attr, out _);
        if (number < 0)
            number = defaultNumber;

        return number;
    }
    public static string GetStringProperty(this BaseComponent component, string attr, string defaultString)
    {
        string data = component.GetUDAProperty<string>(attr, out _);
        if (string.IsNullOrEmpty(data))
            data = defaultString;

        return data;
    }
    public static T GetAppliedValue<T>(this Dictionary<string, object> appliedValues, string attribute, T defaultValue)
    {
        if (appliedValues.TryGetValue(attribute, out object value))
        {
            System.ComponentModel.TypeConverter converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(T));
            if (typeof(T) == typeof(string))
            {
                string value1 = value as string;
                if (string.IsNullOrEmpty(value1))
                    return defaultValue;
                else
                    return (T)converter.ConvertTo(value, typeof(T));
            }

            if (typeof(T) == typeof(int))
            {
                int value1 = (int)value;
                if (value1 < 0)
                    return defaultValue;
                else
                    return (T)converter.ConvertTo(value, typeof(T));
            }

            if (typeof(T) == typeof(double))
            {
                double value1 = (double)value;
                if (value1 < 0)
                    return defaultValue;
                else
                    return (T)converter.ConvertTo(value, typeof(T));
            }
        }
        return defaultValue;
    }
}
