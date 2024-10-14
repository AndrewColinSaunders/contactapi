using System.ComponentModel.DataAnnotations;

namespace contactsapi.Models
{
    public class User
    {
        [Key]
        public int Id
        {
            get;

            set;
        }

        [Required]
        public string Username
        {
            get;

            set;
        }

        [Required]
        public string PasswordHash
        {
            get;

            set;
        }
    }
}