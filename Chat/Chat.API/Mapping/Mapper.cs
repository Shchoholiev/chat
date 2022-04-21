using AutoMapper;
using Chat.API.SignalR;
using Chat.Application.DTO;
using Chat.Core.Entities;
using Chat.Core.Entities.Identity;

namespace Chat.API.Mapping
{
    public class Mapper
    {
        private readonly IMapper _mapper = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<UserDTO, User>();

            cfg.CreateMap<User, UserDTO>();

            cfg.CreateMap<RoomDTO, Room>();

            cfg.CreateMap<Message, SignalRMessage>();

        }).CreateMapper();

        public User Map(User user, UserDTO userDTO)
        {
            return this._mapper.Map(userDTO, user);
        }

        public Room Map(RoomDTO roomDTO)
        {
            return this._mapper.Map<Room>(roomDTO);
        }

        public SignalRMessage Map(Message message)
        {
            return this._mapper.Map<SignalRMessage>(message);
        }
    }
}
