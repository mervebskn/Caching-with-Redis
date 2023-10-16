namespace Redis.Sentinel
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public int Stock { get; set; }
        public Product(int id,string name,string desc,int stock)
        {
            ProductId = id;
            ProductName = name;
            ProductDescription = desc;
            Stock = stock;
        }
        public List<Product> ProductList() {
            Product pro = new Product(1,"Phone","color red",5);
            Product pro2 = new Product(2,"Phone2","color blue",5);
            Product pro3 = new Product(3,"Phone3","color white",5);
            Product pro4 = new Product(4,"Phone4","color black",5);
            List<Product> prolist = new List<Product>();
            prolist.Add(pro); prolist.Add(pro2); 
            prolist.Add(pro3); prolist.Add(pro4);
            return prolist;
        }
    }
}
