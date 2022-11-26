using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using ToDoMauiBlazor.Data;
using ToDoMauiBlazor.Model;
using ToDoMauiBlazor.Services;

namespace ToDoMauiBlazor.Pages
{
    public partial class Index
    {
        //private protected ToDoService _toDoService;

        private bool showFinished = false;

        [Inject]
#pragma warning disable CS8618 // Ein Non-Nullable-Feld muss beim Beenden des Konstruktors einen Wert ungleich NULL enthalten. Erwägen Sie die Deklaration als Nullable.
        public IToDoService ToDoService { get; set; }
#pragma warning restore CS8618 // Ein Non-Nullable-Feld muss beim Beenden des Konstruktors einen Wert ungleich NULL enthalten. Erwägen Sie die Deklaration als Nullable.
        private List<ToDoList>? ToDoLists { get; set; }
        private List<ToDoTask>? ToDoTasks { get; set; }
        private string DisplayListAddPopup { get; set; } = "display-off";
        public bool ListAddPopupIsActive { get; set; } = false;
        private string DisplayListUpdatePopup { get; set; } = "display-off";
        public bool ListUpdatePopupIsActive { get; set; } = false;
        private string DisplayListDeletePopup { get; set; } = "display-off";
        public bool ListDeletePopupIsActive { get; set; } = false;
        private string DisplayTaskAddPopup { get; set; } = "display-off";
        public bool TaskAddPopupIsActive { get; set; } = false;
        private string DisplayTaskDeletePopup { get; set; } = "display-off";
        public bool TaskDeletePopupIsActive { get; set; } = false;
        private bool ShowFinished { get => showFinished; set => ShowFinished_OnPropertyChanged(value); }
        public bool ListIsNotSelected { get; set; } = true;
        public bool TaskIsNotSelected { get; set; } = true;
        public ToDoList? SelectedToDoList { get; set; }
        public ToDoList? NewToDoList { get; set; }
        private string UpdatedListName { get; set; } = string.Empty;
        public ToDoTask? SelectedToDoTask { get; set; }
        public ToDoTask? NewToDoTask { get; set; }

        InputText? inputTextAddList;
        InputText? inputTextUpdateList;
        InputText? inputTextAddTask;

        protected override void OnInitialized()
        {
            RefreshLists();
            SelectList(null);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await SetFocusAsync();
            await base.OnAfterRenderAsync(firstRender);
        }

        /// <summary>
        /// Set focus to input field if a popup is open.
        /// </summary>
        /// <returns></returns>
        async Task SetFocusAsync()
        {
            if (inputTextAddList?.Element is not null && ListAddPopupIsActive)
            {
                await inputTextAddList.Element.Value.FocusAsync();
            }
            if (inputTextUpdateList?.Element is not null && ListUpdatePopupIsActive)
            {
                await inputTextUpdateList.Element.Value.FocusAsync();
            }
            if (inputTextAddTask?.Element is not null && TaskAddPopupIsActive)
            {
                await inputTextAddTask.Element.Value.FocusAsync();
            }
        }

        /// <summary>
        /// If Property ShowFinished is changed, value wil be set, also if a task is selected it will be deselected, if not visible anymore.
        /// </summary>
        /// <param name="value">new value for showFinished</param>
        private void ShowFinished_OnPropertyChanged(bool value)
        {
            showFinished = value;

            if (SelectedToDoTask is null) { return; }

            if (SelectedToDoTask.Finished && !ShowFinished)
            {
                SelectTask(null);
                NewToDoTask = null;
            }
        }

        #region list

        #region list_add

        #region list_add_popup

        /// <summary>
        /// If button "Add" from section "List" is clicked, a new empty list is created, and a popup is shown to enter the new list name.
        /// </summary>
        private void ButtonAddList_OnClick()
        {
            NewToDoList = new();
           
            ShowAddListPopup();
        }

        /// <summary>
        /// If button "Close" from add list popup is clicked, the popup is closed
        /// </summary>
        private void ButtonAddListClose_OnClick()
        {
            HideAddListPopup();
        }

        /// <summary>
        /// If button "Abort" from add list popup is clicked, the popup is closed
        /// </summary>
        private void ButtonAddListAbort_OnClick()
        {
            HideAddListPopup();
        }

        /// <summary>
        /// If button "Ok" from add list popup is clicked, the new list is added to the database. 
        /// If adding was successfully the new list is selected and the table refreshed. Also the task is deselected, the task table cleared, and the detail view is cleared. Last, the popup is closed.
        /// </summary>
        private void ButtonAddListOk_OnClick()
        {
            ToDoList? newToDoList = AddListToDatabase(NewToDoList);

            if (newToDoList is null) { return; }

            SelectList(newToDoList);
            RefreshLists();
            SelectTask(null);
            RefreshTasks(null);
            NewToDoTask = null;
            HideAddListPopup();
        }

        /// <summary>
        /// Property for display of add list popup is set to on.
        /// </summary>
        private void ShowAddListPopup()
        {
            ListAddPopupIsActive = true;
            DisplayListAddPopup = "display-on";
        }
      
        /// <summary>
        /// Property for display of add list popup is set to off.
        /// </summary>
        private void HideAddListPopup()
        {
            ListAddPopupIsActive = false;
            DisplayListAddPopup = "display-off";
        }

        #endregion list_add_popup

        /// <summary>
        /// Add the send new list to the database.
        /// </summary>
        /// <param name="list">list that should be added</param>
        /// <returns>
        /// returns the new list if adding was successfully.
        /// null, if no list was send.
        /// null, if list could not be added to database.
        /// </returns>
        public ToDoList? AddListToDatabase(ToDoList? list)
        {
            if (list is null) { return null; }

            if (!ListNameIsValid(list.Name)) { return null; }

            return ToDoService.CreateList(list);
        }

        #endregion list_add

        #region list_delete

        #region list_delete_popup

        /// <summary>
        /// If button "Delete" from section "List" is clicked, a popup is shown to acknowledge as long as any list is selected.
        /// </summary>
        private void ButtonDeleteList_OnClick()
        {
            if (SelectedToDoList is null) { return; }

            ShowDeleteListPopup();
        }

        /// <summary>
        /// If button "Close" from delete list popup is clicked, the popup is closed
        /// </summary>
        private void ButtonDeleteListClose_OnClick()
        {
            HideDeleteListPopup();
        }

        /// <summary>
        /// If button "Abort" from delete list popup is clicked, the popup is closed
        /// </summary>
        private void ButtonDeleteListAbort_OnClick()
        {
            HideDeleteListPopup();
        }

        /// <summary>
        /// If button "Ok" from delete list popup is clicked, the selected list is deleted from the database. 
        /// If delete was successfully, the list table is refreshed, no list selected, task table is cleared, no task selected, detail view is cleared, and the popup is closed.
        /// </summary>
        private void ButtonDeleteListOk_OnClick()
        {
            if (DeleteListFromDatabase(SelectedToDoList))
            {
                SelectList(null);
                RefreshLists();
                SelectTask(null);
                RefreshTasks(null);
                NewToDoTask = null;
                HideDeleteListPopup();
            }
        }

        /// <summary>
        /// Property for display of delete list popup is set to on.
        /// </summary>
        private void ShowDeleteListPopup()
        {
            ListDeletePopupIsActive = true;
            DisplayListDeletePopup = "display-on";
        }

        /// <summary>
        /// Property for display of delete list popup is set to off.
        /// </summary>
        private void HideDeleteListPopup()
        {
            ListDeletePopupIsActive = false;
            DisplayListDeletePopup = "display-off";
        }

        #endregion list_delete_popup

        /// <summary>
        /// Deletes the list with the send name from the database. 
        /// </summary>
        /// <param name="list">name of the list to delete</param>
        /// <returns>
        /// true, if deletion was successfully.
        /// false, if send list was null.
        /// false, if deletion has failed.
        /// </returns>
        private bool DeleteListFromDatabase(ToDoList? list)
        {
            if (list is null) { return false; }

            return ToDoService.DeleteList(list);
        }

        #endregion list_delete

        #region list_update

        #region list_update_popup

        /// <summary>
        /// If button "Update" from section "List" is clicked, a popup is shown to change the list name. Input field is filled with the actual list name.
        /// Do nothing if no list is selected.
        /// </summary>
        private void ButtonUpdateList_OnClick()
        {
            if (SelectedToDoList is null) { return; }

            UpdatedListName = SelectedToDoList.Name;
            ShowUpdateListPopup();
        }

        /// <summary>
        /// If button "Close" from update list popup is clicked, the popup is closed
        /// </summary>
        private void ButtonUpdateListClose_OnClick()
        {
            HideUpdateListPopup();
        }

        /// <summary>
        /// If button "Abort" from update list popup is clicked, the popup is closed
        /// </summary>
        private void ButtonUpdateListAbort_OnClick()
        {
            HideUpdateListPopup();
        }

        /// <summary>
        /// If button "Ok" from update list popup is clicked, the list is updated in the database. 
        /// If no list is selected, nothing happens.
        /// If the new name is the same as the actual, nothing happens.
        /// If update was successfully, the list table is refreshed, the updated list is selected, and the popup is closed.
        /// </summary>
        private void ButtonUpdateListOk_OnClick()
        {
            if (SelectedToDoList is null) { return; }
            if (SelectedToDoList.Name == UpdatedListName) { return; }

            ToDoList? updatedList = UpdateListInDatabase(SelectedToDoList, UpdatedListName);

            if (updatedList is null) { return; }

            SelectList(updatedList);
            HideUpdateListPopup();
        }

        /// <summary>
        /// Property for display of update list popup is set to on.
        /// </summary>
        private void ShowUpdateListPopup()
        {
            ListUpdatePopupIsActive = true;
            DisplayListUpdatePopup = "display-on";
        }

        /// <summary>
        /// Property for display of update list popup is set to off. Name vor new list is cleared.
        /// </summary>
        private void HideUpdateListPopup()
        {
            ListUpdatePopupIsActive = false;
            UpdatedListName = string.Empty;
            DisplayListUpdatePopup = "display-off";
        }

        #endregion list_update_popup

        /// <summary>
        /// Update the list name in the database.
        /// /// </summary>
        /// <param name="list">list to update</param>
        /// <param name="newListName">new list name</param>
        /// <returns>
        /// true, if list was added successfully.
        /// false, if list was null.
        /// false, if new list name was not valid.
        /// false, if list could not be updated.
        /// </returns>
        private ToDoList? UpdateListInDatabase(ToDoList? list, string newListName)
        {
            if (list is null) { return null; }
            if (!ListNameIsValid(newListName)) { return null; }

            list.Name = newListName;

            return ToDoService.UpdateList(list);
        }

        #endregion list_update

        /// <summary>
        /// If row is clicked the according list will be selected.
        /// If list is null, nothing happens.
        /// If clicked list is already selected, nothing happens.
        /// If new list is selected, task table and detail view is cleared
        /// </summary>
        /// <param name="list">list to selected.</param>
        private void RowList_OnClick(ToDoList? list)
        {
            if (list is null) { return; }
            if (SelectedToDoList == list) { return; }

            SelectList(list);
            SelectTask(null);
            NewToDoTask = null;
        }

        /// <summary>
        /// If row is double clicked, the update popup will show up with the name of the selected list.
        /// </summary>
        /// <param name="list">list to update</param>
        private void RowList_OnDoubleClick(ToDoList? list)
        {
            if (list is null) { return; }

            UpdatedListName = list.Name;
            ShowUpdateListPopup();
        }

        /// <summary>
        /// Updates the ToDoLists. Orders the list by name.
        /// </summary>
        private void RefreshLists()
        {
            ToDoLists = ToDoService.ReadAllLists();

            if (ToDoLists is null) { return; }

            ToDoLists = ToDoLists.OrderBy(tdl => tdl.Name, new NaturalSortComparer<string>()).ToList();
        }

        /// <summary>
        /// Delesect all lists and select the send list.
        /// If list table is null, nothing happens.
        /// If list is null, no list is selected.
        /// If list is selected, task table is refreshed.
        /// </summary>
        /// <param name="list">list that should be selected</param>
        private void SelectList(ToDoList? list)
        {
            if (ToDoLists is null) { return; }

            // deselect all lists
            ToDoLists.ForEach(tdl => tdl.Selected = false);

            if (list is null)
            {
                SelectedToDoList = null;
                ListIsNotSelected = true;
                return;
            }

            list.Selected = true;
            SelectedToDoList = list;
            ListIsNotSelected = false;
            RefreshTasks(list);
        }

        /// <summary>
        /// Checks if send list name is null, or empty and if it is already existing.
        /// </summary>
        /// <param name="name">list name to validate</param>
        /// <returns>true, if name is valid.</returns>
        public bool ListNameIsValid(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) { return false; }
            if (ListExist(name)) { return false; }
            return true;
        }

        /// <summary>
        /// Checks if list with the same name is already existing.
        /// </summary>
        /// <param name="name">list name to check</param>
        /// <returns>true if list exist.</returns>
        public bool ListExist(string name)
        {
            ToDoList? list = ToDoLists?.Where(tdl => tdl.Name == name).FirstOrDefault();

            if (list is null) { return false; }

            return true; ;
        }

        #endregion list

        #region task

        #region task_add

        #region task_add_popup

        /// <summary>
        /// If button "Add" from section "Task" is clicked, a new empty task is created and popup is shown to enter the new task name.
        /// </summary>
        private void ButtonAddTask_OnClick()
        {
            NewToDoTask = new();
            ShowAddTaskPopup();
        }

        /// <summary>
        /// If button "Close" from add task popup is clicked, the popup is closed
        /// </summary>
        private void ButtonAddTaskClose_OnClick()
        {
            HideAddTaskPopup();
        }

        /// <summary>
        /// If button "Abort" from add task popup is clicked, the popup is closed
        /// </summary>
        private void ButtonAddTaskAbort_OnClick()
        {
            HideAddTaskPopup();
        }

        /// <summary>
        /// If button "Ok" from add task popup is clicked, the new task is added to the database.
        /// If adding was successfully the task table and view is refreshed, the new task is selected, and the popup is closed.
        /// </summary>
        private void ButtonAddTaskOk_OnClick()
        {
            ToDoTask? toDoTask = AddTaskToDatabase(SelectedToDoList, NewToDoTask);

            if (toDoTask is null) { return; }

            SelectTask(toDoTask);
            RefreshTasks(SelectedToDoList);
            HideAddTaskPopup();
        }

        /// <summary>
        /// Property for display of add task popup is set to on.
        /// </summary>
        private void ShowAddTaskPopup()
        {
            TaskAddPopupIsActive = true;
            DisplayTaskAddPopup = "display-on";
        }

        /// <summary>
        /// Property for display of add task popup is set to off.
        /// </summary>
        private void HideAddTaskPopup()
        {
            TaskAddPopupIsActive = false;
            DisplayTaskAddPopup = "display-off";
        }

        #endregion task_add_popup

        /// <summary>
        /// Add task to list and update list in database.
        /// </summary>
        /// <param name="list">list were the task should be added to</param>
        /// <param name="task">task that should be added</param>
        /// <returns>
        /// returns the added task.
        /// null, if list was null.
        /// null, if task was null.
        /// null, if task name was not valid.
        /// null, if task could not be added.
        /// </returns>
        private ToDoTask? AddTaskToDatabase(ToDoList? list, ToDoTask? task)
        {
            if (list is null) { return null; }

            if (task is null) { return null; }

            if (!TaskNameIsValid(task.Name)) { return null; }

            task.LastChanged = DateTime.Now;
            list.ToDoTasks.Add(task);

            if (ToDoService.UpdateList(list) is null) { return null; }

            return task;
        }

        #endregion task_add

        #region task_delete

        #region task_delete_popup

        /// <summary>
        /// If button "Delete" from section "Task" is clicked, a popup is shown to acknowledge.
        /// If no task is selected nothing happens.
        /// </summary>
        private void ButtonDeleteTask_OnClick()
        {
            if (SelectedToDoTask is null) { return; }

            ShowDeleteTaskPopup();
        }

        /// <summary>
        /// If button "Close" from delete task popup is clicked, the popup is closed
        /// </summary>
        private void ButtonDeleteTaskClose_OnClick()
        {
            HideDeleteTaskPopup();
        }

        /// <summary>
        /// If button "Abort" from delete task popup is clicked, the popup is closed
        /// </summary>
        private void ButtonDeleteTaskAbort_OnClick()
        {
            HideDeleteTaskPopup();
        }

        /// <summary>
        /// If button "Ok" from delete task popup is clicked, the selected task is deleted from the database. 
        /// If delete was successfully, the task list is refreshed, no task will be selected, and the popup is closed.
        /// </summary>
        private void ButtonDeleteTaskOk_OnClick()
        {
            if (!DeleteTaskFromDatabase(SelectedToDoTask)) { return; }

            SelectTask(null);
            RefreshTasks(SelectedToDoList);
            HideDeleteTaskPopup();
        }

        /// <summary>
        /// Property for display of delete task popup is set to on.
        /// </summary>
        private void ShowDeleteTaskPopup()
        {
            TaskDeletePopupIsActive = true;
            DisplayTaskDeletePopup = "display-on";
        }

        /// <summary>
        /// Property for display of delete task popup is set to off.
        /// </summary>
        private void HideDeleteTaskPopup()
        {
            TaskDeletePopupIsActive = false;
            DisplayTaskDeletePopup = "display-off";
        }

        #endregion task_delete_popup

        /// <summary>
        /// Deletes the send task from the according list and database.
        /// </summary>
        /// <param name="task">task to delete</param>
        /// <returns>
        /// true, if deletion was successfully.
        /// false, if task is null.
        /// false, if task could not be deleted.
        /// </returns>
        private bool DeleteTaskFromDatabase(ToDoTask? task)
        {
            if (task is null) { return false; }

            return ToDoService.DeleteTask(task);
        }

        #endregion task_delete

        #region task_update

        /// <summary>
        /// If button "Update" from section "Details" is clicked, the task will be updated with the new data.
        /// If update was successfully, the updated task is selected and detail view is updated, as long as the task is visible in table.
        /// Otherwise no task is selected and detail view is cleared.
        /// </summary>
        private void ButtonUpdateTask_OnClick()
        {
            ToDoTask? toDoTask = UpdateTaskInDatabase(SelectedToDoTask, NewToDoTask);

            if (toDoTask is null) { return; }

            if (ShowFinished && toDoTask.Finished || !toDoTask.Finished)
            {
                SelectTask(toDoTask);
                RefreshTasks(SelectedToDoList);
            }
            else
            {
                SelectTask(null);
                NewToDoTask = null;
                RefreshTasks(SelectedToDoList);
            }
        }

        ///// <summary>
        ///// If enter is pressed inside of the task name input field, the task is updated in the list/database.
        ///// If update was successfully, the updated task is selected and detail view is updated, as long as the task is visible in table.
        ///// Otherwise no task is selected and detail view is cleared.
        ///// </summary>
        ///// <param name="e">Keyboard event, that includes the pressed key</param>
        //private void InputUpdateTaskName_OnKeyUp(KeyboardEventArgs e)
        //{
        //    if (e.Code != "Enter" && e.Code != "NumpadEnter") { return; }

        //    ToDoTask? toDoTask = UpdateTaskInDatabase(SelectedToDoTask, NewToDoTask);

        //    if (toDoTask is null) { return; }

        //    if (ShowFinished && toDoTask.Finished || !toDoTask.Finished)
        //    {
        //        SelectTask(toDoTask);
        //    }
        //    else
        //    {
        //        SelectTask(null);
        //    }
        //}

        /// <summary>
        /// Update the task in database.
        /// </summary>
        /// <param name="actualTask">actual task</param>
        /// <param name="newTask">new task</param>
        /// <returns>
        /// returns the task, if update was successfully.
        /// returns null, if the actual task was null.
        /// returns null, if the new task was null.
        /// returns null, if list table was empty.
        /// returns null, if no changes between both task were made.
        /// returns null, if new task name was not valid.
        /// returns null, if list with task could not be updated.
        /// </returns>
        private ToDoTask? UpdateTaskInDatabase(ToDoTask? actualTask, ToDoTask? newTask)
        {
            if (actualTask is null) { return null; }
            if (newTask is null) { return null; }

            // get list according to task
            ToDoList? toDoList = ToDoLists?.Where(tdl => tdl.ToDoTasks.Any(tdt => tdt == actualTask)).FirstOrDefault();

            // check list
            if (toDoList is null) { return null; }

            // check if task has changed
            bool TaskNameChanged = actualTask.Name != newTask.Name;
            bool TaskNoteChanged = actualTask.Description != newTask.Description;
            bool TaskFinishedChanged = actualTask.Finished != newTask.Finished;

            bool TaskHasChanged = TaskNameChanged || TaskNoteChanged || TaskFinishedChanged;

            if (!TaskHasChanged) { return null; }

            if (TaskNameChanged)
            {
                // check if new task name is valid
                if (!TaskNameIsValid(newTask.Name)) { return null; }
            }

            // update task
            actualTask.Name = newTask.Name;
            actualTask.Description = newTask.Description;
            actualTask.Finished = newTask.Finished;
            actualTask.LastChanged = DateTime.Now;

            // update list in database, that contains the task
            if (ToDoService.UpdateList(toDoList) is null) { return null; }

            return actualTask;
        }

        #endregion task_update

        /// <summary>
        /// Refresh the task table according to the send list. Tasks are sorted by last changed date.
        /// If task is null, table is cleared.
        /// </summary>
        /// <param name="list">list, that contains the tasks</param>
        private void RefreshTasks(ToDoList? list)
        {
            if (list is null)
            {
                ToDoTasks = null;
                return;
            }

            ToDoTasks = list.ToDoTasks.OrderByDescending(tdt => tdt.LastChanged).ToList();
        }

        /// <summary>
        /// Deselect all task and select the send task.
        /// If task table is empty, nothing happen.
        /// If task is null, no task will be selected.
        /// </summary>
        /// <param name="task">task to select</param>
        private void SelectTask(ToDoTask? task)
        {
            if (ToDoTasks is null) { return; }

            // deselect all tasks 
            ToDoTasks.ForEach(tdt => tdt.Selected = false);

            if (task is null)
            {
                SelectedToDoTask = null;
                TaskIsNotSelected = true;
                return;
            }

            task.Selected = true;
            SelectedToDoTask = task;
            NewToDoTask = new()
            {
                Name = task.Name,
                Description = task.Description,
                Finished = task.Finished,
                Selected = task.Selected,
                LastChanged = task.LastChanged
            };
            TaskIsNotSelected = false;
        }

        /// <summary>
        /// If row is clicked the according task will be selected.
        /// </summary>
        /// <param name="task">task to select</param>
        private void RowTask_OnClick(ToDoTask task)
        {
            SelectTask(task);
        }

        /// <summary>
        /// Check if task already exist in the task table.
        /// </summary>
        /// <param name="name">task name to look for</param>
        /// <returns>true if task already exist, otherwise false</returns>
        private bool TaskExist(string name)
        {
            ToDoTask? toDoTask = ToDoTasks?.Where(tdt => tdt.Name == name).FirstOrDefault();

            return toDoTask is not null;
        }

        /// <summary>
        /// Check if task name is valid, or already exist
        /// </summary>
        /// <param name="name">task name to check</param>
        /// <returns>true, if task is valid</returns>
        private bool TaskNameIsValid(string name)
        {
            if (string.IsNullOrEmpty(name)) { return false; }

            return !TaskExist(name);
        }

        #endregion task

        
    }
}
