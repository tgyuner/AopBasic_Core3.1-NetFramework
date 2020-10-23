using Net_Framework_AOP.Business;
using Net_Framework_AOP.Business.Interface;
using Net_Framework_AOP.Core;
using System;

namespace Net_Framework_AOP
{
    class Program
    {
        static void Main(string[] args)
        {
            // Servis örneğini oluşturuyoruz.
            var productService = TransparentProxy<ProductService, IProductService>.GenerateProxy();

            // Servis üzerinden GetProduct metotunu çağırıyoruz.
            var product = productService.GetProduct(1);

            Console.WriteLine("Id: {0}, Name: {1}, Price: {2}", product.Id, product.Name, product.Price);
            Console.ReadLine();
        }
    }
}

//  Via for NetFramework : https://www.gokhan-gokalp.com/aspect-oriented-programming-aop-giris-ve-ornek-bir-proje/
//  bknz: https://www.postsharp.net/
//  https://github.com/tgyuner

