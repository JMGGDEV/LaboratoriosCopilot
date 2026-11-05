using SmartStore.Api.Models;

namespace SmartStore.Api.Services;

public interface IInventoryService
{
	Task<Product?> GetByIdAsync(Guid id);
	Task<bool> DecreaseStockAsync(Guid id, int quantity);
	Task<IEnumerable<Product>> GetCriticalStockProductsAsync();
}
