using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoMauiBlazor.Data;
using ToDoMauiBlazor.Model;

namespace ToDoMauiBlazor.Services
{
	public class ToDoTaskService //: IToDoService<ToDoTask>
	{
        private readonly ToDoContext _context;

        public ToDoTaskService(ToDoContext context)
        {
            _context = context;
        }

        public async Task<int> Create(ToDoTask toDoTask)
		{
            if (toDoTask is null)
            {
                throw new ArgumentNullException(nameof(toDoTask), "sended item was null");
            }
            if (_context.ToDoTasks is null)
			{
                throw new NullReferenceException($"no list in database to insert task");
            }

            try
            {
                _ = await _context.ToDoTasks.AddAsync(toDoTask);
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new DbUpdateException("changes couldn't be stored", ex);
            }
        }

		public async Task<int> Delete(int id)
		{
            ToDoTask? toDoTask = await Read(id);

            if (toDoTask == null)
            {
                throw new NullReferenceException("item to delete not found");
            }
            if (_context.ToDoTasks == null)
            {
                throw new NullReferenceException($"no list in database to remove task from");
            }

            try
            {
                _context.ToDoTasks.Remove(toDoTask);
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new DbUpdateException("changes couldn't be stored", ex);
            }
        }

		public async Task<ToDoTask?> Read(int id)
		{
            if (_context.ToDoTasks == null)
            {
                throw new NullReferenceException($"no list in database to read from");
            }
            return await _context.ToDoTasks.FindAsync(id);
        }

		public async Task<IEnumerable<ToDoTask?>?> ReadAll()
		{
            return await Task.Run(() => _context.ToDoTasks);
        }

        public async Task<int> Update(ToDoTask toDoTask)
		{
            if (toDoTask is null)
            {
                throw new ArgumentNullException(nameof(toDoTask), "sended item was null");
            }

            ToDoTask? taskToChange = await Read(toDoTask.Id);

            if (taskToChange == null)
            {
                throw new NullReferenceException($"no task to change found in database"); 
            }
            if (_context.ToDoTasks == null)
            {
                throw new NullReferenceException($"no list in database to change task from");
            }

            taskToChange = toDoTask;

            try
            {
                _context.ToDoTasks.Update(taskToChange);
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new DbUpdateException("changes couldn't be stored", ex);
            }
        }
	}
}
