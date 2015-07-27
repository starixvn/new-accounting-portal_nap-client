namespace Fanex.NAP.Core.Domain
{
    using System;

    /// <summary>
    /// Base entity.
    /// </summary>
    /// <typeparam name="TId">The type of the id.</typeparam>
    /// <typeparam name="TName">The type of the name.</typeparam>
    public sealed class EntityBase<TId, TName> : IEntity<TId, TName>
    {
        TId IEntity<TId, TName>.Id
        {
            get;
            set;
        }

        TName IEntity<TId, TName>.Name
        {
            get;
            set;
        }
    }
}
