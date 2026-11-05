using Microsoft.AspNetCore.Mvc;
using SmartStore.Api.Models;
using SmartStore.Api.Services;

namespace SmartStore.Api.Controllers;

/// <summary>
/// Provides endpoints for querying and updating product inventory.
/// </summary>
[ApiController]
[Route("api/inventory")]
public sealed class InventoryController(IInventoryService inventoryService) : ControllerBase
{
	/// <summary>
	/// Gets a product by its identifier.
	/// </summary>
	/// <param name="id">The product identifier.</param>
	/// <returns>The product when it exists; otherwise, a not-found response.</returns>
	[HttpGet("{id:guid}")]
	[ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<Product>> GetByIdAsync(Guid id)
	{
		var product = await inventoryService.GetByIdAsync(id);

		return product is null ? NotFound() : Ok(product);
	}

	/// <summary>
	/// Decreases the stock for a product.
	/// </summary>
	/// <param name="id">The product identifier.</param>
	/// <param name="quantity">The quantity to subtract from stock.</param>
	/// <returns>An empty successful response when stock is decreased; otherwise, an error response.</returns>
	[HttpPost("{id:guid}/decrease")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> DecreaseStockAsync(
		Guid id,
		[FromQuery] int quantity)
	{
		if (quantity <= 0)
		{
			return BadRequest("La cantidad a descontar debe ser mayor a 0.");
		}
		var product = await inventoryService.GetByIdAsync(id);
		if (product is null)
		{
			return NotFound();
		}

		var decreased = await inventoryService.DecreaseStockAsync(id, quantity);

		return decreased ? Ok() : BadRequest();
	}

	/// <summary>
	/// Gets products whose stock is at or below the critical threshold.
	/// </summary>
	/// <returns>The products with critical stock levels.</returns>
	[HttpGet("critical")]
	[ProducesResponseType(typeof(IEnumerable<Product>), StatusCodes.Status200OK)]
	public async Task<ActionResult<IEnumerable<Product>>> GetCriticalStockProductsAsync()
	{
		var products = await inventoryService.GetCriticalStockProductsAsync();

		return Ok(products);
	}
}