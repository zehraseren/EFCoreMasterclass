using System.Reflection;
using System.ComponentModel;

namespace StoreFlow.Helpers;

public static class EnumExtensions
{
    public static string GetDescription(this Enum value)
    {
        // Enum tipini alır ve mevcut enum değerinin field bilgisine erişilir
        var field = value.GetType().GetField(value.ToString());

        // Burada field üzerindeki [Description] attribute'unu çekilir
        var attr = field?.GetCustomAttribute<DescriptionAttribute>();

        // Eğer attribute bulunduysa attribute.Description değerini döndürür, bulunamadıysa enum değerinin ismini döndürür
        return attr?.Description ?? value.ToString();
    }

    // String değeri enum'a parse edip açıklamasını (Description) döner
    // TEnum: Enum kısıtlaması ile sadece enum tipleri için kullanılabilir hale getirir
    public static string GetDescription<TEnum>(this string enumString) where TEnum : struct, Enum
    {
        // String'i belirtilen enum tipine parse etmeyi dener
        if (Enum.TryParse<TEnum>(enumString, true, out var enumValue))
        {
            return enumValue.GetDescription();
        }

        return enumString; // Eğer parse edilemezse, orijinal string değeri döner
    }

    // Enum değerinin badge rengini (CSS sınıfı) döndürür
    public static string GetBadgeColor(this Enum value)
    {
        // Enum tipini alır ve mevcut enum değerinin field bilgisine erişilir
        var field = value.GetType().GetField(value.ToString());

        // Burada field üzerindeki [BadgeColor] attribute'unu çekilir
        var attribute = (BadgeColorAttribute)field?.GetCustomAttributes(typeof(BadgeColorAttribute), false).FirstOrDefault();

        // Eğer attribute bulunduysa attribute.Color değerini döndürür, bulunamadıysa varsayılan badge rengini döndürür
        return attribute?.Color ?? "badge badge-secondary badge-pill";
    }


    // String değeri enum'a parse edip ilgili badge CSS sınıfını döner
    public static string GetBadgeColor<TEnum>(this string enumString) where TEnum : struct, Enum
    {
        // String'i belirtilen enum tipine parse etmeyi dener
        if (Enum.TryParse<TEnum>(enumString, true, out var enumValue))
        {
            return enumValue.GetBadgeColor();
        }

        return "badge badge-secondary badge-pill"; // Eğer parse edilemezse varsayılan badge rengini döner
    }
}
