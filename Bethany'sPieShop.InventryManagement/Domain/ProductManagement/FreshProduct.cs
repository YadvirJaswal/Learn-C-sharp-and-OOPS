using Bethany_sPieShop.InventoryManagement.Domain.Contract;
using Bethany_sPieShop.InventoryManagement.Domain.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bethany_sPieShop.InventoryManagement.Domain.ProductManagement
{
    public class FreshProduct : Product , ISaveable
    {
        public DateTime ExpiryDate { get; set; }
        public string? StorageInstructions {  get; set; }
        public FreshProduct(int id, string name, string? description, Price price, UnitType unitType, int maxAmountInStock) 
            : base(id, name, description, price, unitType, maxAmountInStock)
        {
        }
        public override string DisplayFullDetails()
        {
            StringBuilder sb = new StringBuilder();
            
            sb.Append($"{Id} {Name}\n {Description}\n {Price}\n {AmountInStock} items in stock.");
            if (IsBelowThreshold)
            {
                sb.Append("!! Stock Low!!");
            }

            sb.Append($"Storage instructions {StorageInstructions}");
            sb.Append($"Expiry date {ExpiryDate.ToShortDateString}");
            return sb.ToString();
        }
        public override void IncreaseStock()
        {
            AmountInStock++;
        }

        public string ConvertToStringForSaving()
        {
            return $"{Id};{Name};{Description};{maxItemsInStock};{Price.ItemPrice};{Price.Currency};{UnitType};2;";
        }

        public override object Clone()
        {
            return new FreshProduct(0, this.Name, this.Description, new Price()
            {
                ItemPrice = this.Price.ItemPrice,
                Currency = this.Price.Currency
            }, this.UnitType, this.maxItemsInStock);
        }
    }
}
