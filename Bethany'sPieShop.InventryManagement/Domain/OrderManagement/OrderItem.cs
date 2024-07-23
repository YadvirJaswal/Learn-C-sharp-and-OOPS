using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bethany_sPieShop.InventoryManagement.Domain.OrderManagement
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int ProductId {  get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int AmountOrdered { get; set; }

        public override string ToString()
        {
            return $"Product Id: {ProductId} - Name: {ProductName} - Amount Ordered: {AmountOrdered}";
        }
    }
}
