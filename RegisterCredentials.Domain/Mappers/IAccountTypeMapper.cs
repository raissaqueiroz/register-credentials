using RegisterCredentials.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegisterCredentials.Domain.Mappers
{
    public interface IAccountTypeMapper
    {
        AccountType Map(string accountType);
    }
}
