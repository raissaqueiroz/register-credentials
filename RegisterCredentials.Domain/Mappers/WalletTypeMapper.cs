using RegisterCredentials.Domain.Enums;
using RegisterCredentials.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegisterCredentials.Domain.Mappers
{
    public class WalletTypeMapper : IWalletTypeMapper
    {
        public WalletType Map(string scheme, string schemeType)
        {
            string product = schemeType switch
            {
                "Debit" => "D",
                "Credit" => "C",
                "PrePayment" => "C",
                _ => throw new InvalidMappingException(scheme, schemeType, "Invalid SchemeType")
            };

            // TODO: Add Scenario for every value
            string brand = scheme switch
            {
                "AmericanExpress" => "AC",
                "Elo" => "EC",
                "EloDebito" => "EC",
                "Maestro" => "MC",
                "Mastercard" => "MC",
                "MastercardPre" => "MC",
                "Ourocard" => "OC",
                "Verdecard" => "VD",
                "Visa" => "VC",
                "VisaPre" => "VC",
                "VisaElectron" => "VC",
                _ => throw new InvalidMappingException(scheme, schemeType, "Invalid Scheme")
            };

            var unmappedWallet = brand + product;
            if (Enum.TryParse(unmappedWallet, out WalletType wallet) &&
                Enum.IsDefined(typeof(WalletType), wallet))
            {
                return wallet;
            }

            // TODO: Add Scenario for this exception
            throw new InvalidWalletException(unmappedWallet, "Invalid scheme + scheme type combination");
        }

        public WalletType? Map(string bacenCode)
        {
            if (Enum.TryParse<WalletType>(bacenCode, out var walletType))
            {
                return walletType;
            }

            return null;
        }
    }
}
