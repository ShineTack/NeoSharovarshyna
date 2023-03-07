namespace NeoSharovarshyna.Web.Services
{
    public static class ServiceProvider
    {
        public static IServiceCollection AddServiceHttpClients(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient<IProductService, ProductService>("ProductApi",opt =>
            {
                opt.BaseAddress = new Uri(configuration["Apis:ProductApi"]);
            });
            return services;
        }

        public static IServiceCollection AddDataServices(this IServiceCollection services)
        {
            services.AddScoped<IProductService, ProductService>();
            return services;
        }

        public static IServiceCollection ConfigureNeoSharovarshynaServices(this IServiceCollection services, IConfiguration configuration)
        {
            return AddServiceHttpClients(services, configuration)
                .AddDataServices();
        }
    }
}
