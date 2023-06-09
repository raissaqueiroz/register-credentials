using RegisterCredentials.Domain.Enums;

namespace RegisterCredentials.Domain.Mappers
{
    public class DocumentMapper : IDocumentMapper
    {
        public DocumentType Map(string documentNumber)
        {
            return documentNumber.Length switch
            {
                11 => DocumentType.CPF,
                14 => DocumentType.CNPJ,
                _ => throw new ArgumentOutOfRangeException(nameof(documentNumber), "Document length should be either 11 or 14")
            };
        }
    }
}
