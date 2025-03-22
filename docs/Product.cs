namespace Trendbol
{
    public class Product
    {
        public int ProductID{get; set;}
        public string Name{get; set;}
        public string Description{get; set;}
        public float Price{get; set;}
        public int Stock{get; set;}
        public int SellerID{get; set;}
        public string Category{get; set;}
        public Product(int productID, string name, string description, float price, int stock, int sellerID, string category)
        {
            ProductID = productID;
            Name = name;
            Description = description;
            Price = price;
            Stock = stock;
            SellerID = sellerID;
            Category = category;
        }

        // TODO: Fonksiyonlar buraya eklenecek
    }
}
