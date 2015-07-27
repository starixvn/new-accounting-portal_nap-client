namespace Fanex.NAP.Core.Domain
{
    using System;

    public interface IUser : IEntity<int, string>
    {
        IGroup Group { get; set; }

        string Username { get; set; }

        string Password { get; set; }

        DateTime LastLoginDate { get; set; }

        IUser Creator { get; set; }

        DateTime CreatedDate { get; set; }

        IUser Modifier { get; set; }

        DateTime ModifiedDate { get; set; }

        string FirstName { get; set; }

        string LastName { get; set; }

        string Email { get; set; }
    }
}