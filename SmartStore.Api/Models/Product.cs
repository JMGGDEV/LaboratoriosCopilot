namespace SmartStore.Api.Models;
// Entidad Product con Id (Guid), Nombre, Sku, Precio decimal, Stock int, FechaCreacion DateTime
public class Product
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Nombre { get; set; } = string.Empty;
    public string Sku { get; set; } = string.Empty;
    public decimal Precio { get; set; }
    public int Stock { get; set; }
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
}