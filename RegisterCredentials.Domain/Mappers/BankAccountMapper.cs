using Microsoft.Extensions.Logging;
using RegisterCredentials.Domain.Enums;
using Serilog.Context;
using Serilog.Core.Enrichers;

namespace RegisterCredentials.Domain.Mappers
{
    public class BankAccountMapper : IBankAccountMapper
    {
        private readonly ILogger<BankAccountMapper> _logger;

        public BankAccountMapper(ILogger<BankAccountMapper> logger) {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public AccountType? Map(int bankAccountTypeId)
        {
            var accountType = GetBankAccountId(bankAccountTypeId);

            if(accountType is null)
            {
                using var _ = LogContext.Push(
                new PropertyEnricher("accountType", bankAccountTypeId));

                _logger.LogInformation($"BankAccount was not found {nameof(bankAccountTypeId)}");
            }


            return accountType;
        }

        public AccountType? GetBankAccountId(int bankAccountTypeId)
        {
            return bankAccountTypeId switch
            {
                1 => AccountType.CA,
                2 => AccountType.CC,
                _ => null
            };
        }
    }
}
