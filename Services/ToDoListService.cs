using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoMauiBlazor.Data;
using ToDoMauiBlazor.Model;

namespace ToDoMauiBlazor.Services
{
    public class ToDoListService //: IToDoService<ToDoList>
    {
        private readonly ToDoContext _context;

        public ToDoListService(ToDoContext context)
        {
            _context = context;
        }

        public async Task<int> Create(ToDoList toDoList)
        {
            if (toDoList is null)
            {
                throw new ArgumentNullException(nameof(toDoList), "sended item was null");
            }
            if (_context.ToDoLists is null)
            {
                throw new NullReferenceException($"no list in database to insert list");
            }

            try
            {
                _ = await _context.ToDoLists.AddAsync(toDoList);
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new DbUpdateException("changes couldn't be stored", ex);
            }
        }

        public async Task<int> Delete(int id)
        {
            ToDoList? toDoList = await Read(id);

            if (toDoList == null)
            {
                throw new NullReferenceException("item to delete not found");
            }
            if (_context.ToDoLists == null)
            {
                throw new NullReferenceException($"no list in database to remove list from");
            }

            try
            {
                _context.ToDoLists.Remove(toDoList);
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new DbUpdateException("changes couldn't be stored", ex);
            }
        }

        public async Task<ToDoList?> Read(int id)
        {
            if (_context.ToDoLists == null)
            {
                return null;
            }
            return await _context.ToDoLists.FindAsync(id);
        }

        public async Task<IEnumerable<ToDoList?>?> ReadAll()
        {
            return await Task.Run(() => _context.ToDoLists);
        }

        public async Task<int> Update(ToDoList toDoList)
        {
            if (toDoList is null)
            {
                throw new ArgumentNullException(nameof(toDoList), "sended item was null");
            }

            ToDoList? listToChange = await Read(toDoList.Id);

            if (listToChange == null)
            {
                throw new NullReferenceException($"no list to change found in database");
            }
            if (_context.ToDoLists == null)
            {
                return 0;
            }

            listToChange = toDoList;

            try
            {
                _context.ToDoLists.Update(listToChange);
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new DbUpdateException("changes couldn't be stored", ex);
            }
        }
    }
}
