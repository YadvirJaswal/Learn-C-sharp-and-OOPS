using Bethany_sPieShop.InventoryManagement.Domain.Contract;
using Bethany_sPieShop.InventoryManagement.Domain.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bethany_sPieShop.InventoryManagement.Domain.ProductManagement
{
    public class BoxedProduct : Product , ISaveable
    {

        private int amountPerBox;

        public int AmountPerBox
        {
            get
            {
                return amountPerBox;
            }
            set
            {
                amountPerBox = value;
            }
        }
        public BoxedProduct(int id, string name, string? description, Price price, int amountPerBox,
            int maxAmountInStock) : base(id, name, description, price, UnitType.PerBox, maxAmountInStock)
        {
            AmountPerBox = amountPerBox;
        }

        public override string DisplayFullDetails()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Boxed Product\n");
            sb.Append($"{Id}{Name}\n{Description}\n{Price}\n{AmountInStock} items in stock.");
            if (IsBelowThreshold)
            {
                sb.Append("!! Stock Low!!");
            }
            return sb.ToString();
        }
        public override void UseProduct(int items)
        {
            int smallestMultiple = 0;
            int batchSize;
            while (true)
            {
                smallestMultiple++;
                if (smallestMultiple * AmountPerBox > items)
                {
                    batchSize = smallestMultiple* AmountPerBox;
                    break;
                }
            }
            base.UseProduct(batchSize);
        }

        public override void IncreaseStock(int amount)
        {
            int newStock = AmountInStock + amount * AmountPerBox;
            if(newStock <= maxItemsInStock)
            {
                AmountInStock += amount * AmountPerBox;
            }
            else
            {
                AmountInStock = maxItemsInStock;// we only store possible items,overstock is not stored
                Log($"{CreateSimpleProductRepresentation()} stock overflow.{newStock - AmountInStock} items ordered that couldn't" +
                    $"be stored");
            }
            if(AmountInStock > StockThreshold)
            {
                IsBelowThreshold = false;
            }
        }
        public override void IncreaseStock()
        {
            AmountInStock++;
        }

        public string ConvertToStringForSaving()
        {
            return $"{Id};{Name};{Description};{maxItemsInStock};{Price.ItemPrice};{Price.Currency};{UnitType};1;" +
                $"{AmountPerBox}";
        }

        public override object Clone()
        {
            return new BoxedProduct(0, this.Name, this.Description, new Price()
            {
                ItemPrice = this.Price.ItemPrice,
                Currency = this.Price.Currency
            }, this.maxItemsInStock, this.AmountPerBox);
        }
    }
}
