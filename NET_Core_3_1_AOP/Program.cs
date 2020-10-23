using AspectCore.Configuration;
using AspectCore.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using NET_Core_3_1_AOP.Attribute;
using NET_Core_3_1_AOP.Business;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace NET_Core_3_1_AOP
{
    class Program
    {
        static void Main(string[] args)
        {

            // MSDI ServiceCollection
            IServiceCollection serviceDescriptors = new ServiceCollection();
            serviceDescriptors.AddTransient<IAccountService, AccountService>();

            // Add Service to IoC
            // Add Interceptors so Dynamic Proxy can do its work and intercept
            serviceDescriptors.ConfigureDynamicProxy(config =>
            {
                config.Interceptors.AddTyped<LogingInterceptor>(x => x.GetCustomAttribute<LoggerAttribute>() != null);
                config.Interceptors.AddTyped<CacheInterceptor>(x => x.GetCustomAttribute<CacheAttribute>() != null);

            });

            // Normally you build normally(with .Build). 
            var serviceProvider = serviceDescriptors.BuildDynamicProxyProvider();

            // Attention please, use To BuildDynamicProxyProvider 
            // Burda genellikle BuildDynamicProxyProvider kullanılmadığı için aop yapisi calışmıyor.

            // Get AccountService instance from IoC
            var service = serviceProvider.GetService<IAccountService>();
            var service2 = serviceProvider.GetService<IAccountService>();

            // Run method
            //Task.WaitAll(a, b);
            Task.WaitAll(service.Login(), service2.GetAll());

            Console.ReadKey();
        }
    }
}
