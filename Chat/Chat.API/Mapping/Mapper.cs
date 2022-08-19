using AutoMapper;
using Chat.API.SignalR;
using Chat.Application.Models.Dtos;
using Chat.Core.Entities;
using Chat.Core.Entities.Identity;

namespace Chat.API.Mapping
{
    public class Mapper
    {
        private readonly IMapper _mapper = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<UserDto, User>();

            cfg.CreateMap<User, UserDto>();

            cfg.CreateMap<RoomDto, Room>();

            cfg.CreateMap<Message, SignalRMessage>();

        }).CreateMapper();

        public User Map(User user, UserDto userDTO)
        {
            return this._mapper.Map(userDTO, user);
        }

        public Room Map(RoomDto roomDTO)
        {
            return this._mapper.Map<Room>(roomDTO);
        }

        public SignalRMessage Map(Message message)
        {
            return this._mapper.Map<SignalRMessage>(message);
        }
    }
}
