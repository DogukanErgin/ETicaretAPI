using ETicaretAPI.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Repositories
{
    public interface IReadRepository<T> : IRepository<T> where T : BaseEntity   //Irepository constraitlendiği için burası da olmalı
    {
        IQueryable<T> GetAll(bool tracking = true);
        IQueryable<T> GetWhere(Expression<Func<T, bool>> method, bool tracking = true); //orm üzerinden komutla lambda veriyoruz bu lambdanın hangi tür olduğunu algılar ve tüm nesneleri getirir
        Task<T> GetSingleAsync(Expression<Func<T, bool>> method, bool tracking = true); 
        Task<T> GetByIdAsync(string id, bool tracking = true);
        

    }
}
