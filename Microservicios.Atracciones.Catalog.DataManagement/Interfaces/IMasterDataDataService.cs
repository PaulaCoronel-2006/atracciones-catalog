using Microservicios.Atracciones.Catalog.DataAccess.Entities;

namespace Microservicios.Atracciones.Catalog.DataManagement.Interfaces;

public interface IMasterDataDataService
{
    Task<IEnumerable<Tag>> GetAllTagsAsync();
    Task<Tag?> GetTagByIdAsync(Guid id);
    Task<Guid> AddTagAsync(Tag tag);
    Task<bool> UpdateTagAsync(Tag tag);
    Task<bool> DeleteTagAsync(Guid id);

    Task<IEnumerable<InclusionItem>> GetAllInclusionsAsync();
    Task<InclusionItem?> GetInclusionByIdAsync(Guid id);
    Task<Guid> AddInclusionAsync(InclusionItem inclusion);
    Task<bool> UpdateInclusionAsync(InclusionItem inclusion);
    Task<bool> DeleteInclusionAsync(Guid id);

    Task<IEnumerable<Language>> GetAllLanguagesAsync();
    Task<IEnumerable<TicketCategory>> GetAllTicketCategoriesAsync();
    Task<TicketCategory?> GetTicketCategoryByIdAsync(Guid id);
    Task<Guid> AddTicketCategoryAsync(TicketCategory category);
    Task<bool> UpdateTicketCategoryAsync(TicketCategory category);
    Task<bool> DeleteTicketCategoryAsync(Guid id);
}

