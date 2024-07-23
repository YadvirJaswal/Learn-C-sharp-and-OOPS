using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bethany_sPieShop.InventoryManagement.Domain.ProductManagement
{
    public  partial class Product
    {

        public static int StockThreshold = 5;

        public static void ChangeStockThreshold(int newStockThreshold)
        {
            if (newStockThreshold > 0)
            {
                StockThreshold = newStockThreshold;
            }
        }
        public void UpdateLowStock()
        {
            if (AmountInStock < StockThreshold)
            {
                IsBelowThreshold = true;
            }
        }
        protected virtual void Log(string message)
        {
            Console.WriteLine(message);
        }

        protected string CreateSimpleProductRepresentation()
        {
            return $"Product {id} {name}";
        }
    }
}
