using Chat.Core.Entities;
using Chat.Core.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chat.Infrastructure.FluentAPI
{
    public class UserEntityConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasIndex(u => u.Email)
                   .IsUnique();

            builder.HasOne<UserToken>(u => u.UserToken);

            builder.HasMany<Connection>(u => u.Connections)
                   .WithOne(c => c.User);

            builder.HasMany<Room>(u => u.Rooms)
                   .WithMany(r => r.Users);
        }
    } 
}
