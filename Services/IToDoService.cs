using ToDoMauiBlazor.Model;

namespace ToDoMauiBlazor.Services
{
    public interface IToDoService
    {
        public ToDoList? CreateList(ToDoList? toDoList);
        public bool DeleteList(ToDoList? toDoList);
        public List<ToDoList> ReadAllLists();
        public ToDoList? UpdateList(ToDoList? toDoList);
        public bool DeleteTask(ToDoTask? toDoTask);
    }
}
