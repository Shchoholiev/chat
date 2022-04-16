using Chat.Core.Entities;
using Chat.Core.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chat.Infrastructure.FluentAPI
{
    public class ConnectionEntityConfiguration : IEntityTypeConfiguration<Connection>
    {
        public void Configure(EntityTypeBuilder<Connection> builder)
        {
            builder.HasOne<User>(c => c.User)
                   .WithMany(u => u.Connections);
        }
    } 
}
