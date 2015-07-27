namespace Fanex.NAP.Core.Domain
{
    public interface IEntity<TId, TName>
    {
        TId Id
        {
            get;
            set;
        }

        TName Name
        {
            get;
            set;
        }
    }
}
