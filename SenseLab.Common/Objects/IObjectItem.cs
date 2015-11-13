namespace SenseLab.Common.Objects
{
    public interface IObjectItem :
        IItem<string>
    {
        IObject Object { get; }
    }
}
