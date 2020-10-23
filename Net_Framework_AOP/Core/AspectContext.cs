using System;

namespace Net_Framework_AOP.Core
{
    public class AspectContext
    {
        private readonly static Lazy<AspectContext> _Instance = new Lazy<AspectContext>(() => new AspectContext());

        private AspectContext()
        {
        }

        public static AspectContext Instance
        {
            get
            {
                return _Instance.Value;
            }
        }

        public string MethodName { get; set; }
        public object[] Arguments { get; set; }
    }
}
