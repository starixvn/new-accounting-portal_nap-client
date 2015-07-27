namespace Fanex.NAP.Core.Model
{
    using System;
    using Fanex.NAP.Core.Domain;

    /// <summary>
    /// Group model.
    /// </summary>
    public class Group : IGroup
    {
        public Group(User creator = null, User modifier = null)
        {
            Creator = creator;
            Modifier = modifier;
        }

        public short Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }

        public IUser Creator { get; set; }

        public DateTime CreatedDate { get; set; }

        public IUser Modifier { get; set; }

        public DateTime ModifiedDate { get; set; }

        public int NumOfUser { get; set; }
    }
}