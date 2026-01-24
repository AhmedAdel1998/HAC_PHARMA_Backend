using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HAC_Pharma.Application.DTOs;
using HAC_Pharma.Domain.Interfaces;

namespace HAC_Pharma.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    /// <summary>
    /// Get products with pagination and filtering
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<ProductListResponseDTO>> GetProducts(
        [FromQuery] int page = 1,
        [FromQuery] int limit = 20,
        [FromQuery] string? category = null,
        [FromQuery] string? search = null,
        [FromQuery] bool includeInactive = false)
    {
        var result = await _productService.GetProductsAsync(page, limit, category, search, includeInactive);
        return Ok(result);
    }

    /// <summary>
    /// Get a single product by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDTO>> GetProduct(int id)
    {
        var product = await _productService.GetProductByIdAsync(id);
        if (product == null)
            return NotFound(new { message = "Product not found" });

        return Ok(product);
    }

    /// <summary>
    /// Create a new product
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ProductDTO>> CreateProduct([FromBody] CreateProductDTO dto)
    {
        var userId = User.FindFirst("id")?.Value ?? "";
        var product = await _productService.CreateProductAsync(dto, userId);
        return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
    }

    /// <summary>
    /// Update an existing product
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ProductDTO>> UpdateProduct(int id, [FromBody] UpdateProductDTO dto)
    {
        var userId = User.FindFirst("id")?.Value ?? "";
        var product = await _productService.UpdateProductAsync(id, dto, userId);
        if (product == null)
            return NotFound(new { message = "Product not found" });

        return Ok(product);
    }

    /// <summary>
    /// Delete a product
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        var result = await _productService.DeleteProductAsync(id);
        if (!result)
            return NotFound(new { message = "Product not found" });

        return NoContent();
    }

    /// <summary>
    /// Get all product categories
    /// </summary>
    [HttpGet("categories")]
    public async Task<ActionResult<List<ProductCategoryDTO>>> GetCategories()
    {
        var categories = await _productService.GetCategoriesAsync();
        return Ok(categories);
    }

    /// <summary>
    /// Create a new product category
    /// </summary>
    [HttpPost("categories")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ProductCategoryDTO>> CreateCategory([FromBody] CreateCategoryDTO dto)
    {
        var userId = User.FindFirst("id")?.Value ?? "";
        var category = await _productService.CreateCategoryAsync(dto, userId);
        return CreatedAtAction(nameof(GetCategories), category);
    }

    /// <summary>
    /// Delete a product category
    /// </summary>
    [HttpDelete("categories/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> DeleteCategory(int id)
    {
        var result = await _productService.DeleteCategoryAsync(id);
        if (!result)
            return NotFound(new { message = "Category not found or has products" });

        return NoContent();
    }

    /// <summary>
    /// Get drug interactions for a product
    /// </summary>
    [HttpGet("{id}/interactions")]
    public async Task<ActionResult<List<DrugInteractionDTO>>> GetInteractions(int id)
    {
        var interactions = await _productService.GetInteractionsAsync(id);
        return Ok(interactions);
    }
}
