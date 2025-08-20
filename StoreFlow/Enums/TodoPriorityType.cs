using StoreFlow.Helpers;
using System.ComponentModel;

namespace StoreFlow.Enums;

public enum TodoPriorityType
{
    [Description("Düşük Öncelik"), BadgeColor("badge badge-success badge-pill")]
    Low,

    [Description("Orta Öncelik"), BadgeColor("badge badge-warning badge-pill")]
    Medium,

    [Description("Yüksek Öncelik"), BadgeColor("badge badge-danger badge-pill")]
    High
}
