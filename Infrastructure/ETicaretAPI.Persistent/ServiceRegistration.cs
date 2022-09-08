using ETicaretAPI.Application.Abstractions;
using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Application.Repositories.File;
using ETicaretAPI.Domain.Entities.Identity;
using ETicaretAPI.Persistence.Repositories;
using ETicaretAPI.Persistent.Concretes;
using ETicaretAPI.Persistent.Contexts;
using ETicaretAPI.Persistent.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Persistent
{
    public static class ServiceRegistration
    {
        public static void AddPersistanceServices(this IServiceCollection services) //IoC konteynıra ekleme IserviceCollection 
        {
           
           // services.AddSingleton<IProductService,ProductService>(); 
            services.AddDbContext<ETicaretAPIDbContext>(options=>options.UseNpgsql(Configuration.ConnectionString));
            services.AddIdentity<AppUser,AppRole>().AddEntityFrameworkStores<ETicaretAPIDbContext>();
            services.AddScoped<ICostumerReadRepository, CostumerReadRepository>();
            services.AddScoped<ICostumerWriteRepository, CostumerWriteRepository>();
            services.AddScoped<IOrderReadRepository, OrderReadRepository>();
            services.AddScoped<IOrderWriteRepository, OrderWriteRepository>();
            services.AddScoped<IProductReadRepository, ProductReadRepository>();
            services.AddScoped<IProductWriteRepository, ProductWriteRepository>();
            services.AddScoped<IFileWriteRepository, FileWriteRepository>();
            services.AddScoped<IFileReadRepository, FileReadRepository>();
            services.AddScoped<IInvoiceFileReadRepository, InvoiceFileReadRepository>();
            services.AddScoped<IInvoiceFileWriteRepository, InvoiceFileWriteRepository>();
            services.AddScoped<IProductImageFileReadRepository, ProductImageFileReadRepository>();
            services.AddScoped<IProductImageFileWriteRepository, ProductImageFileWriteRepository>();


        }
    }
}
