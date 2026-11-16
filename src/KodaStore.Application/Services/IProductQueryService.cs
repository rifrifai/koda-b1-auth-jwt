using KodaStore.Application.DTOs;

namespace KodaStore.Application.Services;

public interface IProductQueryService
{
    Task<PagedResult<ProductSearchItemResponse>> SearchAsync(
        ProductSearchRequst request, CancellationToken ct
    );
}