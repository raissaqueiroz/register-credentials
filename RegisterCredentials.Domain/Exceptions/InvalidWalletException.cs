namespace RegisterCredentials.Domain.Exceptions
{
    public class InvalidWalletException : Exception
    {
        public string AttemptedValue { get; }

        public InvalidWalletException(string attemptedValue, string message = null) : base(message)
        {
            AttemptedValue = attemptedValue;
        }
    }
}
