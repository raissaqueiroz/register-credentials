using RegisterCredentials.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RegisterCredentials.Domain.Entities
{
    public class CommercialEstablishment : Entity
    {
        public string CompanyName { get; set; }
        public string DocumentNumber { get; set; }
        public DocumentType DocumentType { get; set; }
        public IList<WalletType> PaymentSchemes { get; set; }
        public Domicile BankAccount { get; set; }
        public bool Enabled { get; set; }
    }

    public class Domicile
    {
        public string Branch { get; set; }
        public string Account { get; set; }
        public string AccountDigit { get; set; }
        public AccountType AccountType { get; set; }
        public string Ispb { get; set; }
        public DocumentType DocumentType { get; set; }
        public string DocumentNumber { get; set; }
    }
}
