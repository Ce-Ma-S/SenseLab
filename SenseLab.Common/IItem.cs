namespace SenseLab.Common
{
    public interface IItem<T>
    {
        T Id { get; }
        string Name { get; }
        string Description { get; }
    }
}
