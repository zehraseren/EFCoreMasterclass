namespace StoreFlow.Models;

public class ProductListByCategoryViewModel
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public decimal ProductPrice { get; set; }
    public int ProductStock { get; set; }
    public int CategoryId { get; set; }
    public string CategoryName { get; set; }
}
