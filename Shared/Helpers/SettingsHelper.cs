using Coop.CommonLibrary.Connections;
using Coop.CommonLibrary.Helpers;

namespace Shared.Helpers
{

    public class SettingsHelper
    {
        public SettingsHelper(string section)
        {
            ConnectionHelper settingDetails = ConnectionSettings.GetConnectionDictionary(section);

            this.SettingDetails = settingDetails.CustomSettings;
        }

        public Dictionary<string, string> SettingDetails
        {
            get;
        }
    }

}