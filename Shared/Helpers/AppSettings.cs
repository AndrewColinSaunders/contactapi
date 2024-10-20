using Shared.Models;

namespace Shared.Helpers
{
    public class AppSettings
    {
        public AppSettings()
        {
            this.ContactsDBHelper = new SettingsHelper("ContactsDB");
            this.JWTSettingsHelper = new SettingsHelper("JWTTokenSettings");

            this.ContactConfig = new ContactConfigModel
            {
                Server = this.ContactsDBHelper.SettingDetails["server"],
                SQLUsername = this.ContactsDBHelper.SettingDetails["username"],
                Password = this.ContactsDBHelper.SettingDetails["password"],
                Database = this.ContactsDBHelper.SettingDetails["database"],
                Trusted = bool.Parse(this.ContactsDBHelper.SettingDetails["trusted"]),

                Issuer = this.JWTSettingsHelper.SettingDetails["Issuer"],
                Audience = this.JWTSettingsHelper.SettingDetails["Audience"],
                SecretKey = this.JWTSettingsHelper.SettingDetails["SecretKey"]
            };
        }

        public SettingsHelper ContactsDBHelper
        {
            get;
        }

        public SettingsHelper JWTSettingsHelper
        {
            get;
        }

        public ContactConfigModel ContactConfig
        {
            get;
        }
    }
}