using StoreFlow.Enums;
using System.Reflection;
using System.ComponentModel;

namespace StoreFlow.Helpers;

public static class EnumExtensions
{
    public static string GetDescription(this Enum value)
    {
        // value.GetType() -> enum tipini alır (ör. OrderStatusType)
        // value.ToString() -> enum üye ismini verir (ör. "Delivered")
        // GetField(...) ile o üyenin metadata'sı (FieldInfo) elde edilir
        var field = value.GetType().GetField(value.ToString());

        // Güvenlik: reflection çağrıları beklenmedik null döndürebilir
        // Bu yüzden field null ise enum'un string temsiliyle geri döndürülür
        if (field == null) return value.ToString();

        //  - FieldInfo nesnesi (enum üyesi metadata'sı) üzerinde, belirtilen tipteki (DescriptionAttribute) ilk attribute'u bulur ve döndürür, eğer bu tipte bir attribute tanımlı değilse null döner
        //  - Burada "DescriptionAttribute" .NET'te System.ComponentModel namespace'inde bulunur ve enum üyelerine açıklama (display text) eklemek için kullanılır.
        var attr = field.GetCustomAttribute<DescriptionAttribute>();

        //  - Eğer attr null değilse (yani DescriptionAttribute bulunduysa) attr.Description property'sini döndürür
        //  - attr null ise (attribute yoksa) enum değerinin adını string olarak döndürür (örn. "InTransit")
        return attr != null ? attr.Description : value.ToString();
    }

    // String değeri enum'a parse edip Description'ını döner, parse edilemezse direkt string'in kendisini döner
    public static string GetDescription(this string statusString)
    {
        // String'i OrderStatusType enum'una parse etmeyi dener
        if (Enum.TryParse<OrderStatusType>(statusString, true, out var statusEnum))
        {
            // Parse başarılıysa ilgili enum'un açıklamasını döndürür
            return statusEnum.GetDescription();
        }

        return statusString; // parse edilmezse direkt string döner
    }

    // Enum Badge Color çekme
    public static string GetBadgeColor(this Enum value)
    {
        // Enum tipini alır ve mevcut enum değerinin field bilgisine erişilir
        //    Örn. OrderStatusType.InTransit -> "InTransit" isimli field'ı bulur
        var fi = value.GetType().GetField(value.ToString());

        // Burada field üzerindeki [BadgeColor] attribute'unu çekilir
        //    GetCustomAttributes ile field'a eklenmiş tüm BadgeColorAttribute tipindeki attribute'ları alır FirstOrDefault() ile ilkini döndürür (yoksa null döner)
        var attribute = (BadgeColorAttribute)fi.GetCustomAttributes(typeof(BadgeColorAttribute), false).FirstOrDefault();

        // Eğer attribute bulunduysa attribute.Color değerini döndürür, bulunamadıysa default CSS class "badge badge-secondary badge-pill" döndürülür
        return attribute?.Color ?? "badge badge-secondary badge-pill";
    }


    // String değeri enum'a parse edip ilgili badge CSS sınıfını döner
    public static string GetBadgeColor(this string statusString)
    {
        // String'i OrderStatusType enum'una parse etmeyi dener
        if (Enum.TryParse<OrderStatusType>(statusString, true, out var statusEnum))
        {
            return statusEnum.GetBadgeColor();
        }

        return "badge badge-secondary badge-pill"; // default renk
    }
}
