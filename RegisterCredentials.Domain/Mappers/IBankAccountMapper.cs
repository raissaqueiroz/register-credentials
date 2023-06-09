using RegisterCredentials.Domain.Enums;

namespace RegisterCredentials.Domain.Mappers
{
    public interface IBankAccountMapper
    {
        AccountType Map(int bankAccountTypeId);
        AccountType? GetBankAccountId(int bankAccountTypeId)
    }
}
