using Chat.Application.Interfaces.Repositories;
using Chat.Core.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Chat.Infrastructure.Services.SignalR
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IGenericRepository<User> _usersRepository;

        private readonly IGenericRepository<Connection> _connectionsRepository;

        public ChatHub(IGenericRepository<User> usersRepository,
                       IGenericRepository<Connection> connectionsRepository)
        {
            _usersRepository = usersRepository;
            _connectionsRepository = connectionsRepository;
        }

        public async Task ChooseChatAsync(string newRoomId, string oldRoomId)
        {
            if (int.Parse(oldRoomId) > 0)
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, oldRoomId);
            }
            await Groups.AddToGroupAsync(Context.ConnectionId, newRoomId);
        }

        public override async Task<Task> OnConnectedAsync()
        {
            var email = Context.User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var user = await _usersRepository.GetOneAsync(u => u.Email == email, CancellationToken.None);
            if (user != null)
            {
                var connection = new Connection
                {
                    Id = Context.ConnectionId,
                    User = user,
                };
                _connectionsRepository.Attach(connection);
                await _connectionsRepository.AddAsync(connection, CancellationToken.None);
            }

            return base.OnConnectedAsync();
        }

        public override async Task<Task> OnDisconnectedAsync(Exception? exception)
        {
            var connection = await _connectionsRepository
                .GetOneAsync(c => c.Id == Context.ConnectionId, CancellationToken.None);
            if (connection != null)
            {
                await _connectionsRepository.DeleteAsync(connection, CancellationToken.None);
            }

            return base.OnDisconnectedAsync(exception);
        }
    }
}
