using Base.Application.Categories.Models;
using Base.Application.Common.Interface;
using Base.Application.Common.Models;
using Base.Application.Common.Services;
using Base.Application.Products.Models;
using Base.Domain.Entities.Categories;
using Base.Domain.Entities.Products;
using Base.Domain.Entities.Warehouses;
using Microsoft.EntityFrameworkCore;
using Shared.Common.Exceptions;

namespace Base.Application.Products.Services;

public class ProductService(IApplicationDbContext context)
    : BaseService<Product, ProductDto>(context), IProductService
{
    public async Task<Guid> CreateAsync(CreateProductRequest request, CancellationToken ct = default)
    {
        var product = new Product()
            .Update(
                request.PartNumber,
                request.Name,
                request.Description,
                request.UnitCost,
                request.RetailPrice,
                request.CategoryId);

        await base.CreateAsync(product, ct);

        return product.Id;
    }

    public async Task<Guid> DeleteAsync(Guid productId, CancellationToken ct = default)
    {
        var product = await FindAsync(productId, ct);
        _ = product ?? throw new NotFoundException($"Product with id {productId} not found.");

        await base.DeleteAsync(product, ct);

        return product.Id;
    }

    public Task<List<ProductDto>> GetAllAsync(CancellationToken ct = default)
        => ListAsync(ct);

    public async Task<ProductDetailDto> GetByIdAsync(Guid productId, CancellationToken ct = default)
    {
        var product = await FindAsync(productId, ct);
        _ = product ?? throw new NotFoundException($"Product with id {productId} not found.");

        var category = await Context.Set<Category>()
            .AsNoTracking()
            .Where(c => c.Id == product.CategoryId)
            .Select(c => new CategoryDto
            {
                Id = c.Id,
                CategoryCode = c.CategoryCode,
                Name = c.Name,
                Description = c.Description,
                Type = c.Type,
                DefaultMarkupPercentage = c.DefaultMarkupPercentage
            })
            .FirstOrDefaultAsync(ct);

        var warehouseStocks = await Context.Set<PartLocation>()
            .AsNoTracking()
            .Where(pl => pl.PartId == productId)
            .Select(pl => new WarehouseStockDto
            {
                Id = pl.Id,
                ZoneCode = pl.WarehouseLocation!.ZoneCode,
                Aisle = pl.WarehouseLocation.Aisle,
                Shelf = pl.WarehouseLocation.Shelf,
                Bin = pl.WarehouseLocation.Bin,
                Quantity = pl.QuantityAtLocation
            })
            .ToListAsync(ct);

        return new ProductDetailDto
        {
            Id = product.Id,
            PartNumber = product.PartNumber,
            Name = product.Name,
            Description = product.Description,
            UnitCost = product.UnitCost,
            RetailPrice = product.RetailPrice,
            Category = category,
            WarehouseStocks = warehouseStocks
        };
    }

    public Task<PaginatedResponse<ProductDto>> SearchAsync(PaginationFilter filter, CancellationToken ct = default)
        => PaginatedSearchAsync(filter, ct);

    public async Task<Guid> UpdateAsync(Guid id, UpdateProductRequest request, CancellationToken ct = default)
    {
        var product = await FindAsync(id, ct);
        _ = product ?? throw new NotFoundException($"Product with id {id} not found.");

        product.Update(
            request.PartNumber,
            request.Name,
            request.Description,
            request.UnitCost,
            request.RetailPrice,
            request.CategoryId);

        await base.UpdateAsync(product, ct);

        return product.Id;
    }
}
