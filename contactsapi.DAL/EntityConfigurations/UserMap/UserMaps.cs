using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Models;

namespace contactsapi.DAL.EntityConfigurations.UserMap
{
    public partial class UserMaps : IEntityTypeConfiguration<User>
    {

        public void Configure(EntityTypeBuilder<User> entity)
        {
            entity.ToTable("Users", "dbo");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<User> entity);

    }
}
