using System;
using System.Collections.Generic;
using System.Text;
using Application.Common.Extension;
using Application.Common.Models;
using Application.Persistence.Repository;
using Domain.Entities.Products;
using Shared.Common.Exceptions;

namespace Application.Products;

public class ProductService(
        IRepositoryWithEvents<Product> eventRepos,
        IReadRepository<Product> readRepos
    ) : IProductService
{
    private readonly IRepositoryWithEvents<Product> _eventRepos = eventRepos;
    private readonly IReadRepository<Product> _readRepos = readRepos;

    public async Task<Guid> CreateAsync(CreateProductRequest request, CancellationToken ct)
    {
        var product = new Product()
            .Update(
                request.PartNumber,
                request.Name,
                request.Description,
                request.UnitCost,
                request.RetailPrice,
                request.CategoryId
            );

        var result = await _eventRepos.AddAsync(product, ct);

        return result.Id;
    }

    public async Task<Guid> DeleteAsync(Guid productId, CancellationToken ct)
    {
        var product = await _readRepos.GetByIdAsync(productId, ct);

        _ = product ?? throw new NotFoundException($"Product with id {productId} not found.");

        await _eventRepos.DeleteAsync(product, ct);

        return product.Id;
    }

    public async Task<List<ProductDto>> GetAllAsync(CancellationToken ct)
    {
        var products = await _readRepos.ListAsync(new GetAllProducts(), ct);

        return products;
    }

    public async Task<ProductDto> GetByIdAsync(Guid productId, CancellationToken ct)
    {
        var product = await _readRepos.GetByIdAsync(productId, ct);
        _ = product ?? throw new NotFoundException($"Product with id {productId} not found.");

        return new ProductDto
        {
            Id = product.Id,
            PartNumber = product.PartNumber,
            Name = product.Name,
            Description = product.Description,
            UnitCost = product.UnitCost,
            RetailPrice = product.RetailPrice,
            CategoryId = product.CategoryId
        };
    }

    public async Task<PaginatedResponse<ProductDto>> SearchAsync(PaginationFilter filter, CancellationToken ct)
    {
        var spec = new ProductPaginated(filter);
        var result = await _readRepos.PaginatedListAsync(spec, filter.PageNumber, filter.PageSize, ct);

        return result;
    }

    public async Task<Guid> UpdateAsync(UpdateProductRequest request, CancellationToken ct)
    {
        var product = await _readRepos.GetByIdAsync(request.Id, ct);
        _ = product ?? throw new NotFoundException($"Product with id {request.Id} not found.");

        product.Update(
            request.PartNumber,
            request.Name,
            request.Description,
            request.UnitCost,
            request.RetailPrice,
            request.CategoryId
        );

        await _eventRepos.UpdateAsync(product, ct);

        return product.Id;
    }
}
