namespace CeMaS.Common.Units
{
    public interface ISIUnit<T> :
        IUnit<T>
    {
        ISIPrefix Prefix { get; }
    }
}