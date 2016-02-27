using CeMaS.Common.Identity;
using CeMaS.Common.Validation;

namespace SenseLab.Common.Objects
{
    public class ObjectInfo :
        Identity<string>,
        IObjectInfo
    {
        #region Init

        public ObjectInfo(
            ObjectEnvironmentInfo environment,
            string id,
            IdentityInfo info,
            ObjectType type,
            ObjectInfo parent = null
            ) :
            base(id, info)
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
                (IdentityInfo)value.Info,
                (ObjectType)value.Type,
                value.Parent == null ?
                    null :
                    new ObjectInfo(value.Parent)
                )
        { }

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

        public ObjectEnvironmentInfo Environment { get; }
        IObjectEnvironmentInfo IObjectInfo.Environment
        {
            get { return Environment; }
        }
        public ObjectInfo Parent { get; }
        IObjectInfo IObjectInfo.Parent
        {
            get { return Parent; }
        }

        #endregion
    }
}
