using System;
using System.Windows;
using System.Windows.Data;

namespace Tekla.Extension.TplClasses;
[ValueConversion(typeof(Enum), typeof(string[]))]
public class EnumValuesConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        return value == null ? Binding.DoNothing : Enum.GetValues((Type)value);
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

[ValueConversion(typeof(Enum), typeof(string))]
public class EnumToStringValueConverter : DependencyObject, IValueConverter//TODO localize
{
    #region IValueConverter Members
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        if (value == null) return Binding.DoNothing;
        return ((Enum)value).StringValue();
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        throw new NotImplementedException();
    }
    #endregion
}
public static class StringValueExtensions
{
    public static string StringValue(this Enum This)
    {
        System.Reflection.FieldInfo fieldInfo = This.GetType().GetField(This.ToString());
        StringValueAttribute[] attribs = fieldInfo.GetCustomAttributes(typeof(StringValueAttribute), false) as StringValueAttribute[];
        return attribs.Length == 0 ? null : attribs[0].StringValue;
    }
}

[global::System.AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public sealed class StringValueAttribute : Attribute
{
    public StringValueAttribute(string stringValue)
    {
        StringValue = stringValue;
    }
    public string StringValue { get; private set; }
}
