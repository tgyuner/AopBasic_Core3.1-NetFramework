using Net_Framework_AOP.Core.Interface;
using System;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;

namespace Net_Framework_AOP.Core
{
    public class TransparentProxy<T, TI> : RealProxy where T : TI, new() // İlgili tipin örneğini alabilmek için new() constraint'ini ekliyoruz.
    {
        private TransparentProxy()
            : base(typeof(TI)) // RealProxy sınıfına, gerçek nesnemizi temsil edecek olan tipi veriyoruz, ilgili interface'imiz.
        {

        }

        // İlgili tipte RealProxy aracılığı ile proxy'mizi oluşturuyoruz.
        public static TI GenerateProxy()
        {
            var instance = new TransparentProxy<T, TI>();
            return (TI)instance.GetTransparentProxy();
        }

        // İlgili metot çağırıldığında çalışacak olan metotdur.
        public override IMessage Invoke(IMessage msg)
        {
            var methodCallMessage = msg as IMethodCallMessage;
            ReturnMessage returnMessage = null;

            try
            {
                // tipimiz üzerinden metot infoya erişerek ilgili attribute olarak eklenmiş
                // aspect'lerimizi buluyoruz.
                var realType = typeof(T);
                var mInfo = realType.GetMethod(methodCallMessage.MethodName);
                var aspects = mInfo.GetCustomAttributes(typeof(IAspect), true);

                // Hatırlarsanız aspect'lerimiz içerisinde kullanmıştık. Gelen parametreleri dolduruyoruz context'e.
                FillAspectContext(methodCallMessage);

                object response = null;

                // Before aspectlerimizi çalıştırıyoruz önce ve geriye değer dönen varsa respons'a eşitliyoruz.
                CheckBeforeAspect(response, aspects);

                object result = null;

                // Response boş değilse, buradaki veri cache üzerinden de geliyor olabilir ve tekrardan invoke etmeye
                // gerek yok, direkt olarak geriye response dönebiliriz bu durumda.
                if (response != null)
                {
                    returnMessage = new ReturnMessage(response, null, 0, methodCallMessage.LogicalCallContext, methodCallMessage);
                }
                else
                {
                    // Response boş ise ilgili metot'u artık invoke ederek çalıştırıyor ve sonucunu alıyoruz.
                    result = methodCallMessage.MethodBase.Invoke(new T(), methodCallMessage.InArgs);
                    returnMessage = new ReturnMessage(result, null, 0, methodCallMessage.LogicalCallContext, methodCallMessage);
                }

                CheckAfterAspect(result, aspects);

                // After aspectlerimizi'de çalıştırdıktan sonra artık geriye çıktıyı dönebiliriz.
                return returnMessage;
            }
            catch (Exception ex)
            {
                return new ReturnMessage(ex, methodCallMessage);
            }
        }

        private void FillAspectContext(IMethodCallMessage methodCallMessage)
        {
            AspectContext.Instance.MethodName = methodCallMessage.MethodName;
            AspectContext.Instance.Arguments = methodCallMessage.InArgs;
        }

        private void CheckBeforeAspect(object response, object[] aspects)
        {
            foreach (IAspect loopAttribute in aspects)
            {
                if (loopAttribute is IBeforeVoidAspect)
                {
                    ((IBeforeVoidAspect)loopAttribute).OnBefore();
                }
                else if (loopAttribute is IBeforeAspect)
                {
                    response = ((IBeforeAspect)loopAttribute).OnBefore();
                }
            }
        }

        private void CheckAfterAspect(object result, object[] aspects)
        {
            foreach (IAspect loopAttribute in aspects)
            {
                if (loopAttribute is IAfterVoidAspect)
                {
                    ((IAfterVoidAspect)loopAttribute).OnAfter(result);
                }
            }
        }
    }
}
