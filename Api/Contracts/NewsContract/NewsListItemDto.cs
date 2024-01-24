using Domain.Models.Common;

namespace Api.Contracts.NewsContract;

public record NewsListItemDto(
    Guid Id,
    string Title,
    string Description,
    string Url,
    StorageMedia Image,
    DateTime DateTime,
    bool IsDeleted);