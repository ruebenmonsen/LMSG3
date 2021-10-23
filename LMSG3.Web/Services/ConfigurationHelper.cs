using Microsoft.Extensions.Configuration;

namespace LMSG3.Web.Services
{
    public partial class LiteraturesController
    {
        public static class ConfigurationHelper
        {
            public static string GetByName(string configKeyName)
            {
                var config = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build();

                IConfigurationSection section = config.GetSection(configKeyName);

                return section.Value;
            }
        }
    }
}
