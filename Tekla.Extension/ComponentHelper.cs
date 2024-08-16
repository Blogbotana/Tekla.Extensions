using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tekla.Structures;
using Tekla.Structures.Geometry3d;
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
    public static Point[] GetInputPointsOfComponent(Component component)
    {
        List<Point> inputPoints = new();
        ComponentInput originalInput = component.GetComponentInput();
        if (originalInput is null)
            return Array.Empty<Point>();


        foreach (object inputItem in originalInput)
        {
            if (inputItem is not InputItem item)
                continue;

            switch (item.GetInputType())
            {
                case InputItem.InputTypeEnum.INPUT_1_POINT:
                    inputPoints.Add(item.GetData() as Point);
                    break;
                case InputItem.InputTypeEnum.INPUT_2_POINTS or InputItem.InputTypeEnum.INPUT_POLYGON:
                    {
                        if (item.GetData() is not ArrayList data)
                            break;

                        inputPoints.AddRange(data.Cast<Point>());
                        break;
                    }
                default:
                    break;
            }
        }
        return inputPoints.ToArray();
    }
    public static IReadOnlyCollection<ModelObject> GetInputObjectsOfComponent(Component component)
    {
        var model = new Tekla.Structures.Model.Model();
        List<ModelObject> inputObjects = new();
        ComponentInput originalInput = component.GetComponentInput();
        if (originalInput is null)
            return Array.Empty<ModelObject>();

        foreach (object inputItem in originalInput)
        {
            if (inputItem is not InputItem item)
                continue;

            switch (item.GetInputType())
            {
                case InputItem.InputTypeEnum.INPUT_1_OBJECT:
                    var inputObject = item.GetData() as ModelObject;
                    if (inputObject is not null && inputObject.Identifier.ID != 0)
                        inputObjects.Add(inputObject);
                    break;
                case InputItem.InputTypeEnum.INPUT_N_OBJECTS:
                    {
                        if (item.GetData() is not ArrayList data)
                            break;

                        inputObjects.AddRange(data.Cast<ModelObject>()
                            .Where(o => o is not null && o.Identifier.ID != 0));
                        break;
                    }
                default:
                    break;
            }
        }
        return inputObjects;
    }
}
