using Net_Framework_AOP.Business.Model;

namespace Net_Framework_AOP.Business.Interface
{
    public interface IProductService
    {
        Product GetProduct(int productId);
    }
}
