using contactsapi.DAL.BaseClasses;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace contactsapi.DAL.ContextConfigurations
{

    public partial class ContactsContext : DbContext
    {
        public ContactsContext()
        {
        }

        public ContactsContext(DbContextOptions<ContactsContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ConnectionBase.GetDataBaseSettings("ContactsDB"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Contact>()
            .HasKey(lu => new { lu.Id });

            modelBuilder.Entity<User>()
           .HasKey(lu => new { lu.Id });

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(typeof(ContactsContext)));

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
