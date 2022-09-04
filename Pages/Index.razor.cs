using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;
using ToDoMauiBlazor.Data;
using ToDoMauiBlazor.Model;
using ToDoMauiBlazor.Services;

namespace ToDoMauiBlazor.Pages
{
    public partial class Index
    {
        [Inject]
        public ToDoListService ListService { get; set; }
        [Inject]
        public ToDoTaskService TaskService { get; set; }

        public IEnumerable<ToDoList?>? ToDoLists { get; set; }
        public IEnumerable<ToDoTask?>? ToDoTasks { get; set; }

        [Required]
        [Range(typeof(bool), "true", "true",
        ErrorMessage = "This form disallows unapproved ships.")]
        public bool ShowFinished { get; set; }

        protected async override Task OnInitializedAsync()
        {
            ToDoLists = await ListService.ReadAll();
            ToDoTasks = await TaskService.ReadAll();
            ShowFinished = false;
        }


    }
}
