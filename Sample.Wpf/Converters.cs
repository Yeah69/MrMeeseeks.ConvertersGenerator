using MrMeeseeks.ConvertersGenerator;

namespace Sample.Wpf;

[WpfConverters]
internal static partial class Converters
{
    internal static object Convert(object value, Type targetType)
    {
        return value;
    }
}