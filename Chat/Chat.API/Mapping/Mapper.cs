using AutoMapper;
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

            cfg.CreateMap<RoomDTO, Room>();

        }).CreateMapper();

        public User Map(User user, UserDTO userDTO)
        {
            return this._mapper.Map(userDTO, user);
        }

        public Room Map(RoomDTO roomDTO)
        {
            return this._mapper.Map<Room>(roomDTO);
        }
    }
}
