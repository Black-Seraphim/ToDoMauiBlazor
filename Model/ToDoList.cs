using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoMauiBlazor.Model
{
    public class ToDoList
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<ToDoTask> ToDoTasks { get; set; }
    }
}
