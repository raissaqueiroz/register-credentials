namespace RegisterCredentials.Domain.Exceptions
{
    public class InvalidMappingException :Exception
    {
        public InvalidMappingException(string scheme, string schemeType, string message = null) : base(message)
        {
            Scheme = scheme;
            SchemeType = schemeType;
        }

        public string Scheme { get; }
        public string SchemeType { get; }
    }
}
