using AspectCore.DynamicProxy;
using System.Threading.Tasks;

namespace NET_Core_3_1_AOP.Attribute
{
    public class CacheAttribute : System.Attribute
    {

    }

    public class CacheInterceptor : IInterceptor
    {
        public bool AllowMultiple => true;

        public bool Inherited { get; set; }

        public int Order { get; set; }

        public Task Invoke(AspectContext context, AspectDelegate next)
        {
            // Buraya yazılan kodlar before

            return next.Invoke(context).ContinueWith(task =>
            {
                // buraya yazılan kodlar after
            });

        }
    }
}
