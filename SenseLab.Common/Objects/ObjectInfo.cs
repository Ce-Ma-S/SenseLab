using CeMaS.Common.Validation;

namespace SenseLab.Common.Objects
{
    public class ObjectInfo :
        Item<string>,
        IObjectInfo
    {
        #region Init

        public ObjectInfo(
            ObjectEnvironmentInfo environment,
            string id,
            string name,
            ObjectType type,
            string description = null,
            ObjectInfo parent = null
            ) :
            base(id, name, description)
        {
            type.ValidateNonNull(nameof(type));
            environment.ValidateNonNull(nameof(environment));
            Type = type;
            Environment = environment;
            Parent = parent;
        }

        public ObjectInfo(IObjectInfo value) :
            this(
                new ObjectEnvironmentInfo(value.Environment),
                value.Id,
                value.Name,
                value.Type.ToType(),
                value.Description,
                value.Parent == null ?
                    null :
                    new ObjectInfo(value.Parent)
                )
        {
        }

        #endregion

        #region Identification

        public ObjectType Type { get; }
        IObjectType IObjectInfo.Type
        {
            get { return Type; }
        }
        public string Path
        {
            get
            {
                return Parent == null ?
                    Id :
                    ObjectPath.Join(Parent.Path, Id);
            }
        }

        #endregion

        #region Hierarchy

        public ObjectEnvironmentInfo Environment
        {
            get;
        }
        IObjectEnvironmentInfo IObjectInfo.Environment
        {
            get { return Environment; }
        }
        public ObjectInfo Parent
        {
            get;
        }
        IObjectInfo IObjectInfo.Parent
        {
            get { return Parent; }
        }

        #endregion
    }
}
