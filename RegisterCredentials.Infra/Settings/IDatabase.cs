namespace RegisterCredentials.Infra.Settings
{
    public interface IDatabase
    {
        string DatabaseName { get; set; }
        string ConnectionString { get; set; }
    }
}
