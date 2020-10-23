namespace Net_Framework_AOP.Core.Interface
{
    public interface IAfterAspect : IAspect
    {
        object OnAfter(object value);
    }
}
