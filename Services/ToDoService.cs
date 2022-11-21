﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq;
using ToDoMauiBlazor.Data;
using ToDoMauiBlazor.Model;
using ToDoMauiBlazor.Tools;

namespace ToDoMauiBlazor.Services
{
    internal class ToDoService : IToDoService
    {
        private readonly ToDoContext _context;

        public ToDoService(ToDoContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Stores the send ToDo-List in the database.
        /// </summary>
        /// <param name="list">List to store in database</param>
        /// <returns>On success, returns the stored list, otherwise returns null.</returns>
        public ToDoList? CreateList(ToDoList? list)
        {
            // check argument
            if (list is null)
            {
                Log.WriteEntry("sended item was null", nameof(list));
                return null;
            }

            // add new list
            EntityEntry<ToDoList> entityEntry = _context.ToDoLists.Add(list);

            // save changes to database
            try
            {
                return (_context.SaveChanges() > 0) ? entityEntry.Entity : null;
            }
            catch (Exception ex)
            {
                Log.WriteEntry("changes couldn't be stored", ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Deletes the send list from the database.
        /// </summary>
        /// <param name="list">List to delete from database</param>
        /// <returns>true, if delete was successful.</returns>
        public bool DeleteList(ToDoList? list)
        {
            if (list is null)
            {
                return false;
            }

            if (list.ToDoTasks.Count > 0)
            {
                // delete each according task from the database
                foreach (ToDoTask task in list.ToDoTasks)
                {
                    _context.ToDoTasks.Remove(task);
                }

                // save changes to database
                try
                {
                    if (_context.SaveChanges() == 0)
                    {
                        return false;
                    };
                }
                catch (Exception ex)
                {
                    Log.WriteEntry($"not all according tasks could be deleted", ex.Message);
                    return false;
                }
            }

            // delete list from database
            _context.ToDoLists.Remove(list);

            // save changes to database
            try
            {
                return _context.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                Log.WriteEntry("changes couldn't be stored", ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Get all lists from database that includes also the tasks.
        /// </summary>
        /// <returns>Returns all lists</returns>
        public List<ToDoList> ReadAllLists()
        {
            return _context.ToDoLists
                .Include(tdl => tdl.ToDoTasks)
                .ToList();
        }

        /// <summary>
        /// Updates the send list in the database.
        /// </summary>
        /// <param name="list">list to update</param>
        /// <returns>On success, returns the updated list, otherwise returns null.</returns>
        public ToDoList? UpdateList(ToDoList? list)
        {
            // check argument
            if (list is null)
            {
                Log.WriteEntry("sended item was null", nameof(list));
                return null;
            }

            // update list
            EntityEntry<ToDoList> entityEntry = _context.ToDoLists.Update(list);

            // save changes to database
            try
            {
                return (_context.SaveChanges() > 0) ? entityEntry.Entity : null;
            }
            catch (Exception ex)
            {
                Log.WriteEntry("changes couldn't be stored", ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Removes the task from the list and from the database.
        /// </summary>
        /// <param name="task">task to delete</param>
        /// <returns>true, if delete was successful.</returns>
        public bool DeleteTask(ToDoTask? task)
        {
            if (task is null)
            {
                Log.WriteEntry("no task to delete was send");
                return false;
            }

            ToDoList? toDoList = _context.ToDoLists.Where(tdl => tdl.ToDoTasks.Any(tdt => tdt == task)).FirstOrDefault();

            if (toDoList is null)
            {
                Log.WriteEntry("no list that contains the send task");
                return false;
            }

            if (!toDoList.ToDoTasks.Remove(task))
            {
                Log.WriteEntry("task could not be removed from list");
                return false;
            }

            // update list
            if (UpdateList(toDoList) is null)
            {
                Log.WriteEntry("list with removed task could not be updated");
                return false;
            }

            // remove task from database
            if (_context.ToDoTasks.Remove(task) is null)
            {
                Log.WriteEntry("task could not be removed from database");
                return false;
            }

            // save changes to database
            try
            {
                return _context.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                Log.WriteEntry("changes couldn't be stored", ex.Message);
                return false;
            }
        }
    }
}
