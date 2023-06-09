using RegisterCredentials.Domain.Enums;

namespace RegisterCredentials.Domain.Mappers
{
    public class AccountTypeMapper : IAccountTypeMapper
    {
        public AccountType Map(string accountType)
        {
            return accountType switch
            {
                "CreditAccount" => AccountType.CC,
                "SavingsAccount" => AccountType.SA,
                "DepositAccount" => AccountType.CD,
                _ => throw new ArgumentOutOfRangeException(nameof(accountType), "Unknown account type")
            };
        }
    }
}
