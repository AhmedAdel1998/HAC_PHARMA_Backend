using Microsoft.EntityFrameworkCore;
using HAC_Pharma.Application.DTOs;
using HAC_Pharma.Domain.Entities.Products;
using HAC_Pharma.Domain.Interfaces;
using HAC_Pharma.Infrastructure.Data;

namespace HAC_Pharma.Application.Services;

public class ProductService : IProductService
{
    private readonly ApplicationDbContext _context;

    public ProductService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ProductListResponseDTO> GetProductsAsync(int page, int limit, string? category, string? search, bool includeInactive = false)
    {
        var query = _context.Products
            .Include(p => p.ProductCategory)
            .Where(p => !p.IsDeleted);

        // Only filter by IsActive if includeInactive is false
        if (!includeInactive)
        {
            query = query.Where(p => p.IsActive);
        }

        // Filter by category
        if (!string.IsNullOrEmpty(category))
        {
            query = query.Where(p => p.ProductCategory != null && 
                (p.ProductCategory.Name == category || p.ProductCategory.Code == category));
        }

        // Search
        if (!string.IsNullOrEmpty(search))
        {
            var searchLower = search.ToLower();
            query = query.Where(p => 
                p.Name.ToLower().Contains(searchLower) ||
                (p.GenericName != null && p.GenericName.ToLower().Contains(searchLower)) ||
                (p.BrandName != null && p.BrandName.ToLower().Contains(searchLower)) ||
                (p.Description != null && p.Description.ToLower().Contains(searchLower)));
        }

        var total = await query.CountAsync();
        
        var items = await query
            .OrderBy(p => p.Name)
            .Skip((page - 1) * limit)
            .Take(limit)
            .Select(p => MapToProductDTO(p))
            .ToListAsync();

        return new ProductListResponseDTO
        {
            Items = items,
            Total = total,
            Page = page,
            Limit = limit
        };
    }

    public async Task<ProductDTO?> GetProductByIdAsync(int id)
    {
        var product = await _context.Products
            .Include(p => p.ProductCategory)
            .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

        return product == null ? null : MapToProductDTO(product);
    }

    public async Task<ProductDTO> CreateProductAsync(CreateProductDTO dto, string userId)
    {
        var product = new Product
        {
            Name = dto.Name,
            GenericName = dto.NameAr,
            ProductCategoryId = dto.CategoryId,
            Description = dto.Description,
            DosageForm = dto.DosageForm,
            Strength = dto.Strength,
            UnitPrice = dto.PriceSar,
            Manufacturer = dto.Manufacturer,
            RequiresPrescription = dto.RequiresPrescription,
            IsActive = dto.IsActive,
            ImageUrl = dto.Image,
            CreatedBy = userId
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return (await GetProductByIdAsync(product.Id))!;
    }

    public async Task<ProductDTO?> UpdateProductAsync(int id, UpdateProductDTO dto, string userId)
    {
        var product = await _context.Products
            .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

        if (product == null)
            return null;

        product.Name = dto.Name;
        product.GenericName = dto.NameAr;
        product.ProductCategoryId = dto.CategoryId;
        product.Description = dto.Description;
        product.DosageForm = dto.DosageForm;
        product.Strength = dto.Strength;
        product.UnitPrice = dto.PriceSar;
        product.Manufacturer = dto.Manufacturer;
        product.RequiresPrescription = dto.RequiresPrescription;
        product.IsActive = dto.IsActive;
        product.ImageUrl = dto.Image;
        product.UpdatedBy = userId;
        product.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return await GetProductByIdAsync(id);
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        var product = await _context.Products
            .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

        if (product == null)
            return false;

        product.IsDeleted = true;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<ProductCategoryDTO>> GetCategoriesAsync()
    {
        return await _context.ProductCategories
            .Where(c => !c.IsDeleted && c.IsActive)
            .Select(c => new ProductCategoryDTO
            {
                Id = c.Id,
                Name = c.Name,
                Code = c.Code,
                Description = c.Description,
                ProductCount = c.Products.Count(p => !p.IsDeleted && p.IsActive)
            })
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    public async Task<ProductCategoryDTO> CreateCategoryAsync(CreateCategoryDTO dto, string userId)
    {
        var category = new ProductCategory
        {
            Name = dto.Name,
            Code = dto.Code,
            Description = dto.Description,
            CategoryType = ProductCategoryType.Pharmaceutical,
            IsActive = true,
            CreatedBy = userId
        };

        _context.ProductCategories.Add(category);
        await _context.SaveChangesAsync();

        return new ProductCategoryDTO
        {
            Id = category.Id,
            Name = category.Name,
            Code = category.Code,
            Description = category.Description,
            ProductCount = 0
        };
    }

    public async Task<bool> DeleteCategoryAsync(int id)
    {
        var category = await _context.ProductCategories
            .Include(c => c.Products)
            .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);

        if (category == null)
            return false;

        // Don't delete if there are products
        if (category.Products.Any(p => !p.IsDeleted))
            return false;

        category.IsDeleted = true;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<DrugInteractionDTO>> GetInteractionsAsync(int productId)
    {
        // This is a placeholder - in a real implementation, you would have a DrugInteraction entity
        // For now, return an empty list
        await Task.CompletedTask;
        return new List<DrugInteractionDTO>();
    }

    private static ProductDTO MapToProductDTO(Product p)
    {
        return new ProductDTO
        {
            Id = p.Id,
            Name = p.Name,
            NameAr = p.GenericName, // Using GenericName for Arabic temporarily
            CategoryId = p.ProductCategoryId,
            Category = p.ProductCategory?.Name,
            CategoryCode = p.ProductCategory?.Code,
            Description = p.Description,
            Dosage = p.Strength,
            DosageForm = p.DosageForm,
            Strength = p.Strength,
            StockStatus = "available", // Default for now
            PriceSar = p.UnitPrice,
            PriceUsd = p.UnitPrice.HasValue ? p.UnitPrice.Value / 3.75m : null, // Approximate conversion
            Image = p.ImageUrl,
            Manufacturer = p.Manufacturer,
            RequiresPrescription = p.RequiresPrescription,
            IsActive = p.IsActive
        };
    }
}
