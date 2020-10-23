using AspectCore.Configuration;
using AspectCore.DynamicProxy;
using AspectCore.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            // MSDI ServiceCollection
            IServiceCollection serviceDescriptors = new ServiceCollection();

            // Add Service to IoC
            serviceDescriptors.AddTransient<IAccountService, AccountService>();
            // Add Interceptors so Dynamic Proxy can do its work and intercept
            serviceDescriptors.ConfigureDynamicProxy(config => config.Interceptors.AddTyped<LoggerAttribute>());
            // Normally you build normally(with .Build). 
            var serviceProvider = serviceDescriptors.BuildDynamicProxyProvider();

            // Get AccountService instance from IoC
            var service = serviceProvider.GetService<IAccountService>();


            var methodInfo = typeof(IAccountService).GetMethod(nameof(IAccountService.GetById));
            methodInfo.Invoke(service, null);

            // Run method
            service.GetById();

            Console.ReadKey();
        }
    }

    public class LoggerAttribute : AbstractInterceptorAttribute
    {
        public async override Task Invoke(AspectContext context, AspectDelegate next)
        {
            Console.WriteLine("Invoking"); // We are in aspect
            await next(context); // Run the function
        }
    }

    public interface IAccountService
    {
        // Annotate the method declaration
        [Logger]
        public void GetById();
    }

    public class AccountService : IAccountService
    {
        public void GetById()
        {
            Console.WriteLine("GetById");
        }
    }
}
