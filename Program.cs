using System;

namespace ShoppingSystem
{
    static class Store
    {
        public static string storeName = "ShopEase";
        public static void ShowWelcomeMessage()
        {
            Console.WriteLine("Welcome to " + storeName + "!");
        }
    }

    abstract class Product
    {
        public int Productid { get; set; }
        public string Name { get; set; }
        public decimal P_Price { get; set; }
        public int Stock { get; set; }
        public abstract double GetDiscountedPrice(int quantity);

        public Product(int id, string prdname, decimal price, int stk)
        {
            Productid = id;
            Name = prdname;
            P_Price = price;
            Stock = stk;
        }

        public void ShowProductInfo()
        {
            Console.WriteLine($"ID: {Productid} | Name: {Name} | Price: {P_Price} | Stock: {Stock}");
        }
    }

    class ElectronicProduct : Product
    {
        public int WarrantyYears { get; set; }

        public ElectronicProduct(int id, string name, decimal price, int stock, int warranty)
            : base(id, name, price, stock) // yeh base constructor call hai parameters pass kar raha hai jo same hain
        {
            WarrantyYears = warranty;
        }

        public override double GetDiscountedPrice(int quantity)
        {
            double finalPrice = (double)P_Price; // change ki hai data type from decimal to double
            if (quantity >= 2)
                finalPrice *= 0.95; 
            return finalPrice;
        }
    }

    class GroceryProduct : Product
    {
        public string ExpiryDate { get; set; }

        public GroceryProduct(int id, string name, decimal price, int stock, string exp)
            : base(id, name, price, stock)
        {
            ExpiryDate = exp;
        }

        public override double GetDiscountedPrice(int quantity)
        {
            double finalPrice = (double)P_Price;
            if (quantity >= 5)
                finalPrice *= 0.90; 
            return finalPrice;
        }
    }

    public class Customer
    {
        public int CustomerId { get; set; }
        public string Name { get; set; }

        public Customer(int customerId, string name)
        {
            CustomerId = customerId;
            Name = name;
        }

        public void ShowCustomer()
        {
            Console.WriteLine("Welcome, " + Name + "!");
            Console.WriteLine("Happy Shopping at " + Store.storeName + "!\n");
        }
    }

    class Order
    {
        public Customer Customer { get; private set; }
        public Product Product { get; private set; }
        public int Quantity { get; private set; }
        public double TotalBill { get; private set; }

        public Order(Customer cust, Product prd, int qty)
        {
            Customer = cust;
            Product = prd;
            Quantity = qty;
        }

        public void CalculateBill(string coupon = "")
        {
            try
            {
                if (Quantity > Product.Stock)
                    throw new Exception("Not enough stock available!");

                double priceAfterDiscount = Product.GetDiscountedPrice(Quantity);

                TotalBill = priceAfterDiscount * Quantity;

                if (coupon.ToUpper() == "SAVE100")
                {
                    TotalBill -= 100;
                    Console.WriteLine("Coupon Applied: Rs.100 OFF");
                }

                if (TotalBill < 0) TotalBill = 0;

                Product.Stock -= Quantity;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                TotalBill = 0;
            }
        }

        public void ShowOrderSummary()
        {
            Console.WriteLine("\n=== ORDER SUMMARY ===");
            Product.ShowProductInfo();
            Console.WriteLine($"Quantity: {Quantity}");
            Console.WriteLine($"Total Bill: Rs. {TotalBill}\n");
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Store.ShowWelcomeMessage();

            // Create Products OR ismain hum derived classes ka objects bana rahay hain abstract parent class ka nahi
            Product p1 = new ElectronicProduct(1, "Laptop", 50000, 5, 2);
            Product p2 = new GroceryProduct(2, "Rice Bag", 1200, 20, "2026-01-01");
            Product p3 = new GroceryProduct(3, "Cooking Oil", 1900, 20, "2029-01-01");
            Product p4 = new ElectronicProduct(4, "Headphones", 2500, 10, 1);

            Customer customer = new Customer(101, "Habiba Mursaleen");
            customer.ShowCustomer();

            Console.WriteLine("Available Products:");
            p1.ShowProductInfo();
            p2.ShowProductInfo();
            p3.ShowProductInfo();
            p4.ShowProductInfo();

            Console.Write("\nEnter Product ID to Buy: ");
            int id = Convert.ToInt32(Console.ReadLine());

            Product selectedProduct = null;

            if (id == p1.Productid) selectedProduct = p1;
            else if (id == p2.Productid) selectedProduct = p2;
            else if (id == p3.Productid) selectedProduct = p3;
            else if (id == p4.Productid) selectedProduct = p4;
            else
            {
                Console.WriteLine("Invalid Product ID!");
                return;
            }

            Console.Write("Enter Quantity: ");
            int qty = Convert.ToInt32(Console.ReadLine());

            Console.Write("Enter Coupon Code (or press Enter to skip): ");
            string coupon = Console.ReadLine();

            Order order = new Order(customer, selectedProduct, qty);
            order.CalculateBill(coupon);

            order.ShowOrderSummary();

            Console.WriteLine("Thank you for shopping!");
        }
    }
}
