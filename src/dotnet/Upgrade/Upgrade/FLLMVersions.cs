namespace FoundationaLLM.Utility.Upgrade
{
    public class FLLMVersions
    {
        public static Version NextVersion(Version v)
        {
            switch (v.ToString())
            {
                case "0.4.0":
                    return Version.Parse("0.5.0");
                case "0.5.0":
                    return Version.Parse("0.5.1");

                case "0.5.1":
                    return Version.Parse("0.6.0");

                case "0.6.0":
                    return Version.Parse("0.7.0");

                case "0.7.0":
                    return Version.Parse("0.8.0");

                default:
                    return Version.Parse("0.0.0");
            }
        }
    }
}
