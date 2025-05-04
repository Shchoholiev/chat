using Chat.Core.Entities;
using Chat.Core.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chat.Infrastructure.FluentAPI
{
    public class RoomEntityConfiguration : IEntityTypeConfiguration<Room>
    {
        public void Configure(EntityTypeBuilder<Room> builder)
        {
            builder.HasMany<User>(r => r.Users)
                   .WithMany(u => u.Rooms);

            builder.HasMany<Message>(r => r.Messages)
                   .WithOne(m => m.Room);
        }
    } 
}
