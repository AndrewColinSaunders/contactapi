using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace contactsapi.DAL.ContextConfigurations
{
    public partial class ContactsContext
    {
        public DbSet<Contact> Contacts
        {
            get;

            set;
        }

        public DbSet<User> Users
        {
            get;

            set;
        }
    }
}
