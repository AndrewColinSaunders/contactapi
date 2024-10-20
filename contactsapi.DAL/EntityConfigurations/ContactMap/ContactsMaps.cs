using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Models;


namespace contactsapi.DAL.EntityConfigurations.ContactMap
{
    public partial class ContactsMaps : IEntityTypeConfiguration<Contact>
    {

        public void Configure(EntityTypeBuilder<Contact> entity)
        {
            entity.ToTable("Contacts", "dbo");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<Contact> entity);

    }
}
