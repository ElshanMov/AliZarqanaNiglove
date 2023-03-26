using Microsoft.Extensions.DependencyInjection;
using Service.HelperServices.Implementations;
using Service.HelperServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public static class ServiceRegistration 
    {

        public static void AddServiceLayerServices(this IServiceCollection services)
        {
            services.AddScoped<IFileService, FileService>();
        }

    }
}
