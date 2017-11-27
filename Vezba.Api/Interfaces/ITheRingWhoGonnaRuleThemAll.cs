using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vezba.Entity;

namespace Vezba.Api.Interfaces
{
    public interface ITheRingWhoGonnaRuleThemAll<T>
    {
        Task<IQueryable<T>> GetAll();

        Task<T> GetById(int Id);

        Task<bool> Create(T newCreate);

        Task<bool> Edit(T edit);

        Task<bool> Delete(int Id);
    }
}
