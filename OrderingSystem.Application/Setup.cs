using Microsoft.Extensions.DependencyInjection;
using OrderingSystem.Application.Repositories;
using OrderingSystem.Application.Services;

namespace OrderingSystem.Application
{
    public static class Setup
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IBaseRepository, BaseRepository>();
            services.AddScoped<IMenuRepository, MenuRepository>();
            services.AddScoped<IFileRepository, FileRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IReceiptRepository, ReceiptRepository>();

            services.AddScoped<IMenuService, MenuService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<ITableService, TableService>();
            services.AddScoped<IReceiptService, ReceiptService>();
        }
    }
}
