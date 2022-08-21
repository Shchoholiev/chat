using Chat.Application.Exceptions;
using Chat.Application.Interfaces.Repositories;
using Chat.Application.Interfaces.Services;
using Chat.Application.Mapping;
using Chat.Application.Models.Dtos;
using Chat.Application.Paging;
using Chat.Core.Entities;
using Chat.Core.Entities.Identity;
using Chat.Infrastructure.Services.SignalR;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Infrastructure.Services
{
    public class MessagesService : IMessagesService
    {
        private readonly IHubContext<ChatHub> _hubContext;

        private readonly IGenericRepository<User> _usersRepository;

        private readonly IGenericRepository<Room> _roomsRepository;

        private readonly IMessagesRepository _messagesReposiory;

        private readonly Mapper _mapper = new();

        public MessagesService(IHubContext<ChatHub> hubContext, IGenericRepository<User> usersRepository,
                               IGenericRepository<Room> roomsRepository, IMessagesRepository messagesReposiory)
        {
            this._hubContext = hubContext;
            this._usersRepository = usersRepository;
            this._roomsRepository = roomsRepository;
            this._messagesReposiory = messagesReposiory;
        }

        public async Task<PagedList<MessageDto>> GetPageAsync(PageParameters pageParameters, int roomId, 
            string userEmail, CancellationToken cancellationToken)
        {
            var messages = await this._messagesReposiory
                .GetPageAsync(pageParameters, roomId, userEmail, cancellationToken);
            var dtos = this._mapper.Map(messages);

            return dtos;
        }

        public async Task SendAsync(MessageCreateDto messageDTO, string userEmail, CancellationToken cancellationToken)
        {
            var user = await this._usersRepository.GetOneAsync(u => u.Email == userEmail, cancellationToken);
            if (user == null)
            {
                throw new NotFoundException("User");
            }
            var room = await this._roomsRepository.GetOneAsync(messageDTO.RoomId, cancellationToken);        
            if (room == null)
            {
                throw new NotFoundException("Room");
            }

            var message = new Message
            {
                Text = messageDTO.Text,
                SendDateUTC = DateTime.Now.ToUniversalTime(),
                Sender = user,
                Room = room
            };

            if (messageDTO.RepliedTo > 0)
            {
                var repliedTo = await this._messagesReposiory.GetMessageAsync(messageDTO.RepliedTo, cancellationToken);
                message.RepliedTo = repliedTo;
            }
            await this._messagesReposiory.AddAsync(message, cancellationToken);

            var signalrMessage = this._mapper.Map(message);
            await this._hubContext.Clients.Group(room.Id.ToString())
                .SendAsync("MessageSent", signalrMessage, cancellationToken);
        }

        public async Task EditAsync(int id, MessageCreateDto messageDTO, CancellationToken cancellationToken)
        {
            var message = await this._messagesReposiory.GetMessageAsync(id, cancellationToken);
            if (message == null)
            {
                throw new NotFoundException("Message");
            }

            message.Text = messageDTO.Text;
            await this._messagesReposiory.UpdateAsync(message, cancellationToken);

            var signalrMessage = this._mapper.Map(message);
            await this._hubContext.Clients.Group(messageDTO.RoomId.ToString())
                .SendAsync("MessageEdited", signalrMessage, cancellationToken);
        }

        public async Task HideForSenderAsync(int id, CancellationToken cancellationToken)
        {
            var message = await this._messagesReposiory.GetFullMessageAsync(id, cancellationToken);
            if (message == null)
            {
                throw new NotFoundException("Message");
            }

            message.HideForSender = true;
            await this._messagesReposiory.UpdateAsync(message, cancellationToken);

            var signalrMessage = this._mapper.Map(message);
            await this._hubContext.Clients.Group(message.Room.Id.ToString())
                .SendAsync("MessageHiddenForUser", signalrMessage, cancellationToken);
        }

        public async Task ReplyInPersonAsync(string email, string senderEmail, MessageCreateDto messageDTO, 
            CancellationToken cancellationToken)
        {
            var room = await this._roomsRepository.GetOneAsync(r => r.DisplayName == null
                                                               && r.Users.Any(u => u.Email == email)
                                                               && r.Users.Any(u => u.Email == senderEmail),
                                                               cancellationToken);
            if (room == null)
            {
                var currentUser = await _usersRepository.GetOneAsync(u => u.Email == senderEmail, cancellationToken);
                var recipient = await _usersRepository.GetOneAsync(u => u.Email == email, cancellationToken);
                room = new Room
                {
                    DisplayName = null,
                    Users = new List<User> { currentUser, recipient }
                };
                this._roomsRepository.Attach(currentUser, recipient);
                await this._roomsRepository.AddAsync(room, cancellationToken);
            }

            messageDTO.RoomId = room.Id;
            await this.SendAsync(messageDTO, senderEmail, cancellationToken);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var message = await this._messagesReposiory.GetFullMessageAsync(id, cancellationToken);
            if (message == null)
            {
                throw new NotFoundException("Message");
            }

            await this._messagesReposiory.DeleteAsync(message, cancellationToken);
            await this._hubContext.Clients.Group(message.Room.Id.ToString())
                .SendAsync("MessageDeleted", id, cancellationToken);
        }
    }
}
