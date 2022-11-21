using Microsoft.EntityFrameworkCore;
using ToDoMauiBlazor.Model;

namespace ToDoMauiBlazor.Data
{
    /// <summary>
    /// provides the context for the todo database.
    /// </summary>
    public class ToDoContext : DbContext
    {
        public DbSet<ToDoList> ToDoLists { get; set; }
        public DbSet<ToDoTask> ToDoTasks { get; set; }

        public string DbPath { get; }

#pragma warning disable CS8618 // Ein Non-Nullable-Feld muss beim Beenden des Konstruktors einen Wert ungleich NULL enthalten. Erwägen Sie die Deklaration als Nullable.
        public ToDoContext()
#pragma warning restore CS8618 // Ein Non-Nullable-Feld muss beim Beenden des Konstruktors einen Wert ungleich NULL enthalten. Erwägen Sie die Deklaration als Nullable.
        {
            string path = FileSystem.AppDataDirectory;
            DbPath = Path.Join(path, "todo.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlite($"Data Source={DbPath}");
    }
}
