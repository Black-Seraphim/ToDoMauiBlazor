namespace ToDoMauiBlazor.Model
{
    public class ToDoTask
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime LastChanged { get; set; }
        public bool Finished { get; set; }
        public bool Selected { get; set; } = false;
    }
}