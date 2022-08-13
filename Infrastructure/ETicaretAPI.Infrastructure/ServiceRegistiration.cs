﻿using ETicaretAPI.Application.Abstractions.Storage;
using ETicaretAPI.Application.Services;
using ETicaretAPI.Infrastructure.Concretes.Storage;
using ETicaretAPI.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Infrastructure
{
    public static class ServiceRegistiration
    {
        public static void AddInfrastructureServices(this IServiceCollection services) 
        {
            services.AddScoped<IFileService,FileService>();
            services.AddScoped<IStorageService, StorageService>();
          
        }
        public static void AddStorage<T>(this IServiceCollection services) where T: class,IStorage
        {
            services.AddScoped<IStorage, T>();
        }
    }
}
