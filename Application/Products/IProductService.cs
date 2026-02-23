using System;
using System.Collections.Generic;
using System.Text;
using Application.Common.Interface;
using Application.Common.Models;

namespace Application.Products;

public interface IProductService : ITransientService
{
    Task<PaginatedResponse<ProductDto>> SearchAsync(PaginationFilter filter, CancellationToken ct);
    Task<List<ProductDto>> GetAllAsync(CancellationToken ct);
    Task<ProductDto> GetByIdAsync(Guid productId, CancellationToken ct);
    Task<Guid> CreateAsync(CreateProductRequest request, CancellationToken ct);
    Task<Guid> UpdateAsync(UpdateProductRequest request, CancellationToken ct);
    Task<Guid> DeleteAsync(Guid productId, CancellationToken ct);
}
