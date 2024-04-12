namespace MathRoom
{
    public interface IBaseExampleRules { }

    public abstract class BaseExampleRules
    {
        public abstract bool TrySetRules(IBaseExampleRules example);
    }
}