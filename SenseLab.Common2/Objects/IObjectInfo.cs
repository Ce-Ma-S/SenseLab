namespace SenseLab.Common.Objects
{
    public interface IObjectInfo :
        IItem<string>
    {
        #region Identification

        IObjectType Type { get; }
        string Path { get; }

        #endregion

        #region Hierarchy

        IObjectEnvironmentInfo Environment { get; }
        IObjectInfo Parent { get; }

        #endregion
    }
}
