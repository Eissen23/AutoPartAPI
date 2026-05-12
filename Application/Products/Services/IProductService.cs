using System;
using System.Collections.Generic;
using System.Text;
using Base.Application.Common.Interface;
using Base.Application.Common.Models;
using Base.Application.Products.Models;

namespace Base.Application.Products.Services;

public interface IProductService : ITransientService
{
    Task<PaginatedResponse<ProductDto>> SearchAsync(PaginationFilter filter, CancellationToken ct = default);
    Task<List<ProductDto>> GetAllAsync(CancellationToken ct = default);
    Task<ProductDetailDto> GetByIdAsync(Guid productId, CancellationToken ct = default);
    Task<Guid> CreateAsync(CreateProductRequest request, CancellationToken ct = default);
    Task<Guid> UpdateAsync(Guid id, UpdateProductRequest request, CancellationToken ct = default);
    Task<Guid> DeleteAsync(Guid productId, CancellationToken ct = default);
}
