namespace StoreFlow.Helpers;

// BadgeColorAttribute, enum field'larına özel CSS sınıfı gibi görsel bilgiyi tutmak için kullanılır
// Tekil kullanım var (AllowMultiple = false), yani her field'a sadece bir tane renk/badge atanabilir
// UI tarafında enum değerine karşılık gelen bu renk bilgisi alınarak rozet stilini dinamik ayarlamak mümkündür
[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class BadgeColorAttribute : Attribute
{
    // Renk veya CSS sınıfı bilgisini saklayan read-only property
    public string Color { get; }

    // Constructor, attribute oluşturulurken renk bilgisi atanır
    public BadgeColorAttribute(string color)
    {
        Color = color;
    }
}
