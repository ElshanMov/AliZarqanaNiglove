using Data.Repositories.Implementations;
using Data.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public static class ServiceRegistration
    {


        public static void AddDataLayerServices(this IServiceCollection services)
        {
            services.AddScoped<IBrandRepository, BrandRepository>();
            services.AddScoped<ISettingRepository, SettingRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductCommentRepository, ProductCommentRepository>();
            services.AddScoped<IBlogRepository, BlogRepository>();
            services.AddScoped<ISizeRepository, SizeRepository>();
            services.AddScoped<IColorRepository, ColorRepository>();
            services.AddScoped<IContactRepository, ContactRepository>();
            services.AddScoped<IProductImagesRepository, ProductImageRepository>();
            services.AddScoped<IBlogCommentRepository, BlogCommentRepository>();

        }

    }
}
