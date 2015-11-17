namespace SenseLab.Common.Objects
{
    public interface IObjectItem :
        IObjectItemInfo
    {
        new IObject Object { get; }
    }
}
