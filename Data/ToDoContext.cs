using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoMauiBlazor.Model;

namespace ToDoMauiBlazor.Data
{
    public class ToDoContext : DbContext
    {
        public DbSet<ToDoList>? ToDoLists { get; set; }
        public DbSet<ToDoTask>? ToDoTasks { get; set; }

        public string DbPath { get; }

        public ToDoContext()
        {
            string path = FileSystem.AppDataDirectory;
            DbPath = Path.Join(path, "todo.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlite($"Data Source={DbPath}");
    }
}
