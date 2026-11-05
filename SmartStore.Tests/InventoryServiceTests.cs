using System.Reflection;
using SmartStore.Api.Models;
using SmartStore.Api.Services;

namespace SmartStore.Tests;

public class InventoryServiceTests
{
    [Fact]
    public async Task DecreaseStockAsync_ReduceStock_WhenEnoughStockExists()
    {
        var service = new InventoryService();
        var product = GetSeededProducts(service).First(p => p.Stock > 1);
        var quantity = 1;
        var originalStock = product.Stock;

        var result = await service.DecreaseStockAsync(product.Id, quantity);

        Assert.True(result);

        var updatedProduct = await service.GetByIdAsync(product.Id);

        Assert.NotNull(updatedProduct);
        Assert.Equal(originalStock - quantity, updatedProduct.Stock);
    }

    [Fact]
    public async Task DecreaseStockAsync_ReturnsFalse_WhenQuantityExceedsStock()
    {
        var service = new InventoryService();
        var product = GetSeededProducts(service).First(p => p.Stock >= 0);
        var originalStock = product.Stock;

        var result = await service.DecreaseStockAsync(
            product.Id,
            originalStock + 1);

        Assert.False(result);

        var unchangedProduct = await service.GetByIdAsync(product.Id);

        Assert.NotNull(unchangedProduct);
        Assert.Equal(originalStock, unchangedProduct.Stock);
    }

    [Fact]
    public async Task GetCriticalStockProductsAsync_ReturnsOnlyProductsWithStockAtMostFive()
    {
        var service = new InventoryService();
        var expectedProducts = GetSeededProducts(service)
            .Where(p => p.Stock <= 5)
            .Select(p => p.Id)
            .OrderBy(id => id)
            .ToList();

        var result = await service.GetCriticalStockProductsAsync();
        var actualProducts = result
            .Select(p => p.Id)
            .OrderBy(id => id)
            .ToList();

        Assert.All(result, product => Assert.True(product.Stock <= 5));
        Assert.Equal(expectedProducts, actualProducts);
    }

    private static List<Product> GetSeededProducts(InventoryService service)
    {
        var field = typeof(InventoryService).GetField(
            "_products",
            BindingFlags.Instance | BindingFlags.NonPublic);

        Assert.NotNull(field);

        return Assert.IsType<List<Product>>(field!.GetValue(service));
    }
}