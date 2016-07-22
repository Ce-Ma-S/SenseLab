namespace CeMaS.Common.Identity
{
    /// <summary>
    /// Identifiable object with extended information about itself.
    /// </summary>
    /// <typeparam name="T">Identifier type.</typeparam>
    public interface IIdentity<T> :
        IId<T>,
        IIdentityInfo
    { }
}
