using System.ComponentModel;
using System.Reflection;

namespace Application.Utils;

public static class EnumExcentsion
{
    public static string GetDescription(this Enum value)
    {
        FieldInfo? field = value.GetType().GetField(value.ToString());

        if (field is null)
        {
            return value.ToString();
        }

        var description = field.GetCustomAttribute<DescriptionAttribute>()?.Description;

        return description ?? "An Error occured!";
    }
}
