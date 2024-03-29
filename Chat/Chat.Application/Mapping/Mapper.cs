﻿using AutoMapper;
using Chat.Application.Models.Dtos;
using Chat.Application.Models.SignalR;
using Chat.Application.Paging;
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

            cfg.CreateMap<Room, RoomDto>();

            cfg.CreateMap<Message, SignalRMessage>();

            cfg.CreateMap<Message, MessageDto>();

        }).CreateMapper();

        public User Map(User user, UserDto userDTO)
        {
            return _mapper.Map(userDTO, user);
        }

        public UserDto Map(User user)
        {
            return _mapper.Map<UserDto>(user);
        }

        public Room Map(RoomDto roomDTO)
        {
            return _mapper.Map<Room>(roomDTO);
        }

        public RoomDto Map(Room room)
        {
            return _mapper.Map<RoomDto>(room);
        }

        public PagedList<RoomDto> Map(PagedList<Room> rooms)
        {
            var dto = _mapper.Map<PagedList<RoomDto>>(rooms);
            this.MapPagedList(rooms, dto);
            return dto;
        }

        public PagedList<MessageDto> Map(PagedList<Message> messages)
        {
            var dto = _mapper.Map<PagedList<MessageDto>>(messages);
            this.MapPagedList(messages, dto);
            return dto;
        }

        public SignalRMessage Map(Message message)
        {
            return _mapper.Map<SignalRMessage>(message);
        }

        private void MapPagedList(IPagedList source, IPagedList destination)
        {
            destination.TotalPages = source.TotalPages;
            destination.PageNumber = source.PageNumber;
            destination.PageSize = source.PageSize;
        }
    }
}
