using StoreFlow.Helpers;
using System.ComponentModel;

namespace StoreFlow.Enums;


// Bu enum, siparişin farklı durumlarını temsil etmektedir
// Her durum için hem açıklayıcı Description hem de stil bilgisi içeren BadgeColor attribute'u bulunur
// UI tarafında bu attribute'lar kullanılarak, durumlara özel görsel rozetler (badge) oluşturulabilir
public enum OrderStatusType
{
    [Description("Taşıma Durumunda"), BadgeColor("badge badge-warning badge-pill")]
    InTransit,

    [Description("Teslim Edildi"), BadgeColor("badge badge-success badge-pill")]
    Delivered,

    [Description("İptal Edildi"), BadgeColor("badge badge-danger badge-pill")]
    Cancelled,

    [Description("Ödeme Bekleniyor"), BadgeColor("badge badge-primary badge-pill")]
    PaymentPending,

    [Description("Sipariş Alındı"), BadgeColor("badge badge-info badge-pill")]
    OrderReceived
}