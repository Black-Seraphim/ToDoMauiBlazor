using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoMauiBlazor.Model;

namespace ToDoMauiBlazor.Services
{
	public interface IToDoService<T>
	{
        Task<int> Create(T item);
        Task<IEnumerable<T?>?> ReadAll();
        Task<T?> Read(int id);
        Task<int> Update(T item);
		Task<int> Delete(int id);
	}
}
