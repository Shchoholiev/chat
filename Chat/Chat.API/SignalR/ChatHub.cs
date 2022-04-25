using Chat.Application.IRepositories;
using Chat.Core.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Chat.API.SignalR
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IGenericRepository<User> _usersRepository;

        private readonly IGenericRepository<Connection> _connectionsRepository;

        public ChatHub(IGenericRepository<User> usersRepository, 
                       IGenericRepository<Connection> connectionsRepository)
        {
            this._usersRepository = usersRepository;
            this._connectionsRepository = connectionsRepository;
        }

        public async Task ChooseChat(string newRoomId, string oldRoomId)
        {
            if (int.Parse(oldRoomId) > 0)
            {
                await this.Groups.RemoveFromGroupAsync(this.Context.ConnectionId, oldRoomId);
            }
            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, newRoomId);
        }

        public override async Task<Task> OnConnectedAsync()
        {
            var email = this.Context.User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var user = await this._usersRepository.GetOneAsync(u => u.Email == email);
            if (user != null)
            {
                var connection = new Connection
                {
                    Id = this.Context.ConnectionId,
                    User = user,
                };
                this._connectionsRepository.Attach(connection);
                await this._connectionsRepository.AddAsync(connection);
            }

            return base.OnConnectedAsync();
        }

        public override async Task<Task> OnDisconnectedAsync(Exception? exception)
        {
            var connection = await this._connectionsRepository.GetOneAsync(c => c.Id == Context.ConnectionId);
            if (connection != null)
            {
                await this._connectionsRepository.DeleteAsync(connection);
            }

            return base.OnDisconnectedAsync(exception);
        }
    }
}
