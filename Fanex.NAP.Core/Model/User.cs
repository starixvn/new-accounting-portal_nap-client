namespace Fanex.NAP.Core.Model
{
    using System;
    using Fanex.NAP.Core.Domain;    

    /// <summary>
    /// User model.
    /// </summary>
    [Serializable]
    public class User : IUser
    {
        public User(Group group = null, User creator = null, User modifier = null)
        {
            Group = group;
            Creator = creator;
            Modifier = modifier;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public IGroup Group { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public DateTime LastLoginDate { get; set; }

        public IUser Creator { get; set; }

        public DateTime CreatedDate { get; set; }

        public IUser Modifier { get; set; }

        public DateTime ModifiedDate { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }
    }
}