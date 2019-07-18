namespace Clean.Microservice.Serverless.Plugin.Database
{
    public class DatabaseSettingsParameter
    {
        public virtual string Code { get; set; }

        public virtual string Value { get; set; }

        public static DatabaseSettingsParameter Builder(string code, string value)
        {
            return new DatabaseSettingsParameter
            {
                Code = code,
                Value = value
            };
        }
    }
}