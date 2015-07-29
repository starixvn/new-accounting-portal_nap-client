namespace Fanex.NAP.Core.Web.Session
{
    using System.Collections.Generic;
    using Fanex.NAP.Core.Models;

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