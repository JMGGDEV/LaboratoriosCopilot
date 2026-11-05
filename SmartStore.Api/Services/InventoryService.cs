using SmartStore.Api.Models;

namespace SmartStore.Api.Services;

public class InventoryService : IInventoryService
{
	private readonly List<Product> _products =
	[
		new Product
		{
			Nombre = "Teclado mecánico",
			Sku = "TEC-001",
			Precio = 79.99m,
			Stock = 12
		},
		new Product
		{
			Nombre = "Ratón inalámbrico",
			Sku = "RAT-001",
			Precio = 29.99m,
			Stock = 5
		},
		new Product
		{
			Nombre = "Monitor 24 pulgadas",
			Sku = "MON-001",
			Precio = 159.99m,
			Stock = 3
		}
	];

	public Task<Product?> GetByIdAsync(Guid id)
	{
		var product = _products.FirstOrDefault(product => product.Id == id);
		return Task.FromResult(product);
	}

	public Task<bool> DecreaseStockAsync(Guid id, int quantity)
	{
		var product = _products.FirstOrDefault(product => product.Id == id);
		if (product is null || quantity <= 0 || product.Stock < quantity)
		{
			return Task.FromResult(false);
		}

		product.Stock -= quantity;
		return Task.FromResult(true);
	}

	public Task<IEnumerable<Product>> GetCriticalStockProductsAsync()
	{
		IEnumerable<Product> criticalProducts = _products.Where(product => product.Stock <= 5);
		return Task.FromResult(criticalProducts);
	}
}
