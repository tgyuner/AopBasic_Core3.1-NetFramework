using AspectCore.DynamicProxy;
using System;
using System.Threading.Tasks;

namespace NET_Core_3_1_AOP.Attribute
{
    public sealed class LoggerAttribute : System.Attribute
    {
    }

    public class LogingInterceptor : IInterceptor
    {
        public bool AllowMultiple => true;

        public bool Inherited { get; set; }

        public int Order { get; set; }


        public Task Invoke(AspectContext context, AspectDelegate next)
        {
            try
            {
                // Buraya yazılan kodlar before

                return next.Invoke(context).ContinueWith(task =>
                {
                    // buraya yazılan kodlar finish
                });
            }
            catch (Exception ex)
            {
                //log ex yazdır.

                throw;
            }
        }
    }
}
