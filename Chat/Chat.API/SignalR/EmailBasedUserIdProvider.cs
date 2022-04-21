using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Chat.API.SignalR
{
    public class EmailBasedUserIdProvider : IUserIdProvider
    {
        public virtual string GetUserId(HubConnectionContext connection)
        {
            return connection.User?.FindFirst(ClaimTypes.Email)?.Value;
        }
    }
}
