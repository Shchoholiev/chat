using AutoMapper;
using Chat.Application.Models.Dtos;
using Chat.Application.Models.SignalR;
using Chat.Core.Entities;
using Chat.Core.Entities.Identity;

namespace Chat.Application.Mapping
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
            return _mapper.Map(userDTO, user);
        }

        public Room Map(RoomDto roomDTO)
        {
            return _mapper.Map<Room>(roomDTO);
        }

        public SignalRMessage Map(Message message)
        {
            return _mapper.Map<SignalRMessage>(message);
        }
    }
}
