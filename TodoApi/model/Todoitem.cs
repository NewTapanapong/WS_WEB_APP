namespace TodoApi.Models;

public class TodoItem
{
    public int id { get; set; }
    public string title { get; set; }
    public string? Description { get; set; }
    public bool isCompleted { get; set; }
    public DateTime CreatedAt { get; set; }
}
