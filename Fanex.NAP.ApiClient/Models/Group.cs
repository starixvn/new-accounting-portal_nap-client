namespace Fanex.NAP.Core.Models
{
    using System;

    [Serializable]
    public class Group
    {
        public short Id { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }
    }
}