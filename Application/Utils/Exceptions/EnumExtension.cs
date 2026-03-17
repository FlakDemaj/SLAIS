using System.ComponentModel;
using System.Reflection;

namespace Application.Utils.Exceptions;

public static class EnumExtension
{
    public static string GetDescription(this Enum value)
    {
        var field = value.GetType().GetField(value.ToString());

        if (field is null)
        {
            return value.ToString();
        }

        var description = field.GetCustomAttribute<DescriptionAttribute>()?.Description;

        return description ?? "An Error occured!";
    }
}
