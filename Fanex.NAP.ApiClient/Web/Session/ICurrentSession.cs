namespace Fanex.NAP.Core.Web.Session
{
    using Fanex.NAP.Core.Models;
    using System.Collections.Generic;

    public interface ICurrentSession
    {
        string Username { get; set; }

        int UserId { get; set; }

        int Level { get; set; }

        int OnUserId { get; set; }

        string SessionId { get; set; }

        IList<MenuItem> MenuItemList { get; set; }

        void ClearAll();
    }
}