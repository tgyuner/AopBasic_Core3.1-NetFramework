using Net_Framework_AOP.Attributes;
using Net_Framework_AOP.Business.Interface;
using Net_Framework_AOP.Business.Model;
using System.Collections.Generic;

namespace Net_Framework_AOP.Business
{
    public class ProductService : IProductService
    {
        // In memory olarak bir kaç product ekliyorum.
        private static Dictionary<int, Product> _productDict = new Dictionary<int, Product>();

        public ProductService()
        {
            _productDict.Add(1, new Product() { Id = 1, Name = "Arçelik Çaycı", Price = 2500 });
            _productDict.Add(2, new Product() { Id = 2, Name = "Vestel Venüs", Price = 1200 });
        }

        [Cache(DurationInMinute = 10)]
        [Log]
        public Product GetProduct(int productId)
        {
            return _productDict[productId];
        }
    }
}
