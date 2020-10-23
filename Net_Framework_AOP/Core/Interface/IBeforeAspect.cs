namespace Net_Framework_AOP.Core.Interface
{
    interface IBeforeAspect : IAspect
    {
        object OnBefore();
    }
}
