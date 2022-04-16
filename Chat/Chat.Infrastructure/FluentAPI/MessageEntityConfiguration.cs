using Chat.Core.Entities;
using Chat.Core.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chat.Infrastructure.FluentAPI
{
    public class MessageEntityConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.HasOne<Room>(m => m.Room);

            builder.HasOne<User>(m => m.Sender);
        }
    } 
}
