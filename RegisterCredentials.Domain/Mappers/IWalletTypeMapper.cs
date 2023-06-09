using RegisterCredentials.Domain.Enums;

namespace RegisterCredentials.Domain.Mappers
{
    public interface IWalletTypeMapper
    {
        WalletType Map(string scheme, string schemeType);
        WalletType? Map(string bacenCode);
    }
}
