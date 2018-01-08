using Core.Entities;
using System.Collections.Generic;

namespace Core.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
    }
}
