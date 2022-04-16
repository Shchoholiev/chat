﻿using System.ComponentModel.DataAnnotations;

namespace Chat.Core.Entities.Identity
{
    public class User
    {
        [Key]
        public string Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public UserToken UserToken { get; set; }

        public List<Connection>? Connections { get; set; }

        public List<Chat>? Chats { get; set; }
    }
}
