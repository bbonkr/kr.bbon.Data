namespace kr.bbon.Data
{
    public class DatabaseOptions
    {
        public const string Name = "Database";

        public const string ErrorMessage = @"
Please configure 'Database' section for database behavior in your appsettings.json or environment variable.

```json
""Database"" : {
  ""UseSoftDelete"": true
}
```
";

        /// <summary>
        /// Use soft deletion
        /// </summary>
        public bool UseSoftDelete { get; set; } = false;
    }
}
