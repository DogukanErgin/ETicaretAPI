using ETicaretAPI.Application.Abstractions.Hubs;
using ETicaretAPI.SignalR.HubServices;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.SignalR
{
   public static class ServiceRegistiration
    {

        public static void AddSignalRServices(this IServiceCollection collection)
        {
            collection.AddTransient<IProductHubService,ProductHubService>();
            collection.AddSignalR(); //kendisiyle alakalı tüm struct,abstract,interface IoC 'e ekler
        }
    }
}
