using FoundationaLLM.Common.Models.Configuration.Instance;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace FoundationaLLM.Upgrade
{
    public abstract class Upgrade
    {
        public string TypeName { get; set; }

        protected ILoggerFactory _loggerFactory;

        protected InstanceSettings _instanceSettings;
        public Version SourceInstanceVersion { get; set; }

        public Version TargetInstanceVersion { get; set; }

        public Version ObjectStartUpgradeVersion { get; set; }

        public Type SourceType { get; set; }

        public Type TargetType { get; set; }

        protected Dictionary<string, object> _defaultValues;

        public abstract Task UpgradeAsync();

        public abstract Task LoadAsync();

        public abstract Task SaveAsync();

        public abstract Task<object> UpgradeDoWorkAsync(object in_source);


        public Version GetObjectVersion(object target)
        {
            PropertyInfo pi = target.GetType().GetProperty("Version");
            Version version = null;

            if (pi != null)
                version = (Version)pi.GetValue(target);

            if (version == null)
                version = ObjectStartUpgradeVersion;

            return version;
        }

        public void SetDefaultValues(object agent)
        {
            PropertyInfo[] pi = agent.GetType().GetProperties();

            //use reflection to set all the agent properties to default values
            foreach (string val in _defaultValues.Keys)
            {
                PropertyInfo p = agent.GetType().GetProperty(val);

                if (p == null)
                    continue;

                object propVal = p.GetValue(agent);

                switch (p.PropertyType.Name)
                {
                    case "String":
                        //set the value if it is null
                        if (propVal == null)
                        {
                            string dictVal = _defaultValues[val].ToString();

                            if (dictVal.Contains("{"))
                            {
                                //replace the value with the value from the agent
                                string prop = dictVal.Replace("{", "").Replace("}", "");
                                PropertyInfo propInfo = agent.GetType().GetProperty(prop);
                                object newPropVal = propInfo.GetValue(agent);

                                if (newPropVal != null)
                                    dictVal = newPropVal.ToString();
                                else
                                    dictVal = null;
                            }

                            p.SetValue(agent, dictVal);
                        }
                        break;
                    case "DateTime":
                        if (propVal == null)
                        {
                            DateTime dictVal = DateTime.Parse(_defaultValues[val].ToString());

                            if (dictVal == DateTime.MinValue)
                                dictVal = DateTime.Now;

                            p.SetValue(agent, dictVal);
                        }
                        if (propVal != null)
                        {
                            if ((DateTime)propVal == DateTime.MinValue)
                            {
                                DateTime dictVal = DateTime.Parse(_defaultValues[val].ToString());
                                p.SetValue(agent, dictVal);
                            }
                        }
                        break;
                    case "DateTimeOffset":
                        if (propVal == null)
                        {
                            DateTimeOffset dictVal = DateTimeOffset.Parse(_defaultValues[val].ToString());

                            if (dictVal == DateTimeOffset.MinValue)
                                dictVal = DateTimeOffset.Now;

                            p.SetValue(agent, dictVal);
                        }
                        if (propVal != null)
                        {
                            if ((DateTimeOffset)propVal == DateTimeOffset.MinValue)
                            {
                                DateTimeOffset dictVal = DateTimeOffset.Parse(_defaultValues[val].ToString());
                                p.SetValue(agent, dictVal);
                            }
                        }
                        break;
                    case "Int32":
                        break;
                    case "Boolean":
                        break;
                    case "Guid":
                        break;
                    case "List`1":
                        break;
                    case "Dictionary`2":
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
