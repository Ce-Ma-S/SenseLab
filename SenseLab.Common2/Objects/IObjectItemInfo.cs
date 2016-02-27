namespace SenseLab.Common.Objects
{
    public interface IObjectItemInfo :
        IItem<string>
    {
        IObjectInfo Object { get; }
    }
}
