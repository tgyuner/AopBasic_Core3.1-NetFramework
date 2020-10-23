namespace Net_Framework_AOP.Core.Interface
{
    public interface IAfterVoidAspect : IAspect
    {
        void OnAfter(object value);
    }
}
