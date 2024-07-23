using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bethany_sPieShop.InventoryManagement.Domain.OrderManagement
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderFullfilmentDate { get; private set; }
        public List<OrderItem> OrderItems { get;}
        public bool IsOrderFullfilment { get; set; } = false;

        public Order()
        {
            Id = new Random().Next(999999);

            int numberOfSeconds = new Random().Next(99); 
            OrderFullfilmentDate = DateTime.Now.AddSeconds(numberOfSeconds);

            OrderItems = new List<OrderItem>();
        }

        public string ShowOrderDetails()
        {
            StringBuilder orderDetails = new StringBuilder();
            orderDetails.AppendLine($"Order Id {Id}");
            orderDetails.AppendLine($"Order fullfillment date: {OrderFullfilmentDate.ToShortTimeString()}");

            if(OrderItems != null)
            {
                foreach( OrderItem item in OrderItems )
                {
                    orderDetails.AppendLine($"{item.ProductId}.{item.ProductName}:{item.AmountOrdered}");
                }
            }

            return orderDetails.ToString();
        }
    }
}
