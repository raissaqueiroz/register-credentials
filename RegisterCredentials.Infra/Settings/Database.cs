namespace RegisterCredentials.Infra.Settings
{
    public class Database
    {
        public string Name { get; set; }
        public string ?Host { get; set; }
        public string Port { get; set; }
        public string ?User { get; set; }
        public string Password { get; set; }
        public string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(User) || string.IsNullOrEmpty(Password))
                    return $@"mongodb://{Host}:{Port}";
                return $@"mongodb://{User}:{Password}@{Host}:{Port}/{Name}?authSource=admin";
            }
        }
    }
}
