using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Domain.Entities;
using ETicaretAPI.Persistent.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Persistent.Repositories
{
    public class CostumerWriteRepository : WriteRepository<Customer>, ICostumerWriteRepository
    {
        public CostumerWriteRepository(ETicaretAPIDbContext context) : base(context)
        {
        }
    }
}
