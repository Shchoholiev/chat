using Chat.Application.Exceptions;
using Chat.Application.Interfaces.Repositories;
using Chat.Application.Interfaces.Services;
using Chat.Application.Mapping;
using Chat.Application.Models.Chat;
using Chat.Application.Models.Dtos;
using Chat.Application.Paging;
using Chat.Core.Entities;
using Chat.Core.Entities.Identity;
using Chat.Infrastructure.Services.SignalR;
using Microsoft.AspNetCore.SignalR;

namespace Chat.Infrastructure.Services
{
    public class RoomsService : IRoomsService
    {
        private readonly IHubContext<ChatHub> _hubContext;

        private readonly IGenericRepository<Room> _roomsRepository;

        private readonly IGenericRepository<User> _usersRepository;

        private readonly Mapper _mapper = new();

        public RoomsService(IHubContext<ChatHub> hubContext, IGenericRepository<Room> roomsRepository,
                            IGenericRepository<User> usersRepository)
        {
            this._roomsRepository = roomsRepository;
            this._usersRepository = usersRepository;
            this._hubContext = hubContext;
        }

        public async Task AddMemberAsync(AddToRoomModel model, CancellationToken cancellationToken)
        {
            var room = await this._roomsRepository.GetOneAsync(model.RoomId, cancellationToken, r => r.Users);
            var user = await this._usersRepository.GetOneAsync(u => u.Email == model.Email, cancellationToken);
            if (room == null || room.DisplayName == null || user == null)
            {
                throw new NotFoundException("Room");
            }

            room.Users.Add(user);
            var message = new Message
            {
                Text = $"{user.Name} has joined the group.",
                SendDateUTC = DateTime.Now.ToUniversalTime(),
                Room = room,
            };

            this._roomsRepository.Attach(user, message);
            await this._roomsRepository.SaveAsync(cancellationToken);

            var signalrMessage = this._mapper.Map(message);
            await this._hubContext.Clients.Group(room.Id.ToString()).SendAsync("MessageSent", signalrMessage);
        }

        public async Task CreateAsync(RoomDto roomDTO, string userName, CancellationToken cancellationToken)
        {
            var room = this._mapper.Map(roomDTO);
            room.Messages.Add(new Message 
            { 
                Text = $"Chat created by {userName}.", 
                SendDateUTC = DateTime.Now.ToUniversalTime(),
            });
            this._roomsRepository.Attach(room);
            await this._roomsRepository.AddAsync(room, cancellationToken);
        }

        public async Task<RoomDto> GetRoomAsync(int id, CancellationToken cancellationToken)
        {
            var room = await this._roomsRepository.GetOneAsync(id, cancellationToken, r => r.Users);
            if (room == null)
            {
                throw new NotFoundException("Room");
            }
            var dto = _mapper.Map(room);

            return dto;
        }

        public async Task<PagedList<RoomDto>> GetRoomsAsync(PageParameters pageParameters, string userEmail,
            CancellationToken cancellationToken)
        {
            var rooms = await this._roomsRepository.GetPageAsync(pageParameters,
                r => r.Users.Any(u => u.Email == userEmail), cancellationToken);
            foreach (var personalRoom in rooms.Where(r => r.DisplayName == null))
            {
                var index = rooms.FindIndex(r => r.Id == personalRoom.Id);
                var room = await this._roomsRepository
                    .GetOneAsync(personalRoom.Id, cancellationToken, r => r.Users);
                rooms[index].Users = room.Users.Where(u => u.Email != userEmail).ToList();
            }
            var dtos = this._mapper.Map(rooms);

            return dtos;
        }
    }
}
