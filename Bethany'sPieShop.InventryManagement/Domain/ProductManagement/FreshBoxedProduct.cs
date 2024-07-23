using Bethany_sPieShop.InventoryManagement.Domain.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bethany_sPieShop.InventoryManagement.Domain.ProductManagement
{
    public class FreshBoxedProduct : BoxedProduct
    {
        public FreshBoxedProduct(int id, string name, string? description, Price price, int amountPerBox, UnitType unitType,
            int maxAmountInStock) : base(id, name, description, price, amountPerBox, maxAmountInStock)
        {
        }
       
    }
}
