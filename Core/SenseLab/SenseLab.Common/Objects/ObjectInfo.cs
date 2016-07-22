using CeMaS.Common.Collections;
using CeMaS.Common.Identity;
using CeMaS.Common.Validation;
using System.Collections.Generic;

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
            string name,
            ObjectType type,
            string description = null,
            IDictionary<string, object> values = null,
            ObjectInfo parent = null
            ) :
            base(id, name, description, values)
        {
            Argument.NonNull(type, nameof(type));
            Argument.NonNull(environment, nameof(environment));
            Type = type;
            Environment = environment;
            Parent = parent;
        }

        public ObjectInfo(IObjectInfo value) :
            this(
                new ObjectEnvironmentInfo(value.Environment),
                value.Id,
                value.Name,
                (ObjectType)value.Type,
                value.Description,
                value.Values.ToDictionary(),
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
