namespace ToDoMauiBlazor.Model
{
    public class ToDoList
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<ToDoTask> ToDoTasks { get; set; } = new();
        public bool Selected { get; set; } = false;
    }
}
