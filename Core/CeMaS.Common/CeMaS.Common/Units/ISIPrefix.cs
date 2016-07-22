namespace CeMaS.Common.Units
{
    public interface ISIPrefix :
        IUnit<string>
    {
        double Multiplier { get; }
    }
}