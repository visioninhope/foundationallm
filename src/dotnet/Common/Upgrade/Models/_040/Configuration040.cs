namespace FoundationaLLM.Common.Upgrade.Models._040
{
    public class Configuration040
    {
        public Configuration040()
        {
            ConfigurationValues = new Dictionary<string, ConfigurationKeyValue>();

            //add all the configuraiton values here...
            ConfigurationValues.Add("", new ConfigurationKeyValue {  Key = "", Type = "", Value = ""});
        }

        public Dictionary<string, ConfigurationKeyValue> ConfigurationValues { get; set; }
    }
}
