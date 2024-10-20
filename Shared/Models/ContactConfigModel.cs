namespace Shared.Models
{
    public class ContactConfigModel
    {
        public string Server 
        { 
            get; 
            
            set; 
        }

        public string SQLUsername 
        { 
            get;

            set;
        }

        public string Password 
        { 
            get; 

            set; 
        }

        public string Database 
        { 
            get; 

            set;
        }

        public bool Trusted 
        { 
            get; 

            set; 
        }

        public string Issuer 
        { 
            get; 

            set; 
        }

        public string Audience 
        { 
            get; 

            set; 
        }

        public string SecretKey 
        { 
            get;
            
            set;
        }

    }
}