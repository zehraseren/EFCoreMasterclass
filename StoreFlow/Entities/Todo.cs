using StoreFlow.Enums;

namespace StoreFlow.Entities;

public class Todo
{
    public int TodoId { get; set; }
    public string Description { get; set; }
    public TaskStatusType Status { get; set; }
}
