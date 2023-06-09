using RegisterCredentials.Domain.Enums;

namespace RegisterCredentials.Domain.Mappers
{
    public interface IDocumentMapper
    {
        DocumentType Map(string documentNumber);
    }
}
