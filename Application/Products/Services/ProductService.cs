using System;
using System.Collections.Generic;
using System.Text;
using Application.Categories;
using Application.Common.Extension;
using Application.Common.Models;
using Application.PartLocations;
using Application.PartLocations.Specs;
using Application.Persistence.Repository;
using Application.Products.Models;
using Domain.Entities.Categories;
using Domain.Entities.Products;
using Domain.Entities.Warehouses;
using Shared.Common.Exceptions;

namespace Application.Products.Services;

public class ProductService(
        IRepositoryWithEvents<Product> eventRepos,
        IReadRepository<Product> readRepos,
        IReadRepository<Category> categoryRepos,
        IReadRepository<PartLocation> partLocationRepos
    ) : IProductService
{
    private readonly IRepositoryWithEvents<Product> _eventRepos = eventRepos;
    private readonly IReadRepository<Product> _readRepos = readRepos;
    private readonly IReadRepository<Category> _categoryRepos = categoryRepos;
    private readonly IReadRepository<PartLocation> _partLocationRepos = partLocationRepos;

    public async Task<Guid> CreateAsync(CreateProductRequest request, CancellationToken ct = default)
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

    public async Task<Guid> DeleteAsync(Guid productId, CancellationToken ct = default)
    {
        var product = await _readRepos.GetByIdAsync(productId, ct);

        _ = product ?? throw new NotFoundException($"Product with id {productId} not found.");

        await _eventRepos.DeleteAsync(product, ct);

        return product.Id;
    }

    public async Task<List<ProductDto>> GetAllAsync(CancellationToken ct = default)
    {
        var products = await _readRepos.ListAsync(new GetAllProducts(), ct);

        return products;
    }

    public async Task<ProductDetailDto> GetByIdAsync(Guid productId, CancellationToken ct = default)
    {
        var product = await _readRepos.GetByIdAsync(productId, ct);
        _ = product ?? throw new NotFoundException($"Product with id {productId} not found.");

        var category = await _categoryRepos.FirstOrDefaultAsync(new GetCategoryById(product.CategoryId), ct);

        var partLocations = await _partLocationRepos.ListAsync(new GetPartLocationByPartId(productId), ct);


        return new ProductDetailDto
        {
            Id = product.Id,
            PartNumber = product.PartNumber,
            Name = product.Name,
            Description = product.Description,
            UnitCost = product.UnitCost,
            RetailPrice = product.RetailPrice,
            Category = category is null
                ? null
                : category,
            WarehouseStocks = partLocations
        };
    }

    public async Task<PaginatedResponse<ProductDto>> SearchAsync(PaginationFilter filter, CancellationToken ct = default)
    {
        var spec = new ProductPaginated(filter);
        var result = await _readRepos.PaginatedListAsync(spec, filter.PageNumber, filter.PageSize, ct);

        return result;
    }

    public async Task<Guid> UpdateAsync(Guid id, UpdateProductRequest request, CancellationToken ct = default)
    {
        var product = await _readRepos.GetByIdAsync(id, ct);
        _ = product ?? throw new NotFoundException($"Product with id {id} not found.");

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
