using StoreFlow.Helpers;
using System.ComponentModel;

namespace StoreFlow.Enums;

public enum TaskStatusType
{
    [Description("Bekleniyor"), BadgeColor("badge badge-primary badge-pill")]
    Pending,

    [Description("Devam Ediyor"), BadgeColor("badge badge-warning badge-pill")]
    InProgress,

    [Description("Tamamlandı"), BadgeColor("badge badge-success badge-pill")]
    Completed,
}
