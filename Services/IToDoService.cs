using ToDoMauiBlazor.Model;

namespace ToDoMauiBlazor.Services
{
    public interface IToDoService
    {
        /// <summary>
        /// Stores the send ToDo-List in the database.
        /// </summary>
        /// <param name="list">List to store in database</param>
        /// <returns>On success, returns the stored list, otherwise returns null.</returns>
        public ToDoList? CreateList(ToDoList? list);

        /// <summary>
        /// Deletes the send list from the database.
        /// </summary>
        /// <param name="list">List to delete from database</param>
        /// <returns>true, if delete was successful.</returns>
        public bool DeleteList(ToDoList? list);

        /// <summary>
        /// Get all lists from database that includes also the tasks.
        /// </summary>
        /// <returns>Returns all lists</returns>
        public List<ToDoList> ReadAllLists();

        /// <summary>
        /// Updates the send list in the database.
        /// </summary>
        /// <param name="list">list to update</param>
        /// <returns>On success, returns the updated list, otherwise returns null.</returns>
        public ToDoList? UpdateList(ToDoList? list);

        /// <summary>
        /// Removes the task from the list and from the database.
        /// </summary>
        /// <param name="task">task to delete</param>
        /// <returns>true, if delete was successful.</returns>
        public bool DeleteTask(ToDoTask? task);
    }
}
