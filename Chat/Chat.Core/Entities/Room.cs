﻿using Chat.Core.Entities.Identity;

namespace Chat.Core.Entities
{
    public class Room : EntityBase
    {
        public string? DisplayName { get; set; }

        public List<Message>? Messages { get; set; } = new();

        public List<User> Users { get; set; }
    }
}
