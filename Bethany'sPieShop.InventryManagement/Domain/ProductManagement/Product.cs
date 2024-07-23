using Bethany_sPieShop.InventoryManagement.Domain.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bethany_sPieShop.InventoryManagement.Domain.ProductManagement
{
    public abstract partial class Product : ICloneable
    {
        private int id;
        private string name = string.Empty;
        private string? description;

        protected int maxItemsInStock = 0;
        public int Id
        {
            get { return id; }
            set
            {
                id = value;
            }
        }

        public string Name
        {
            get { return name; }
            set
            {
                name = value.Length > 50 ? value[..50] : value;
            }
        }

        public string Description
        {
            get { return description; }
            set
            {
                if (value == null)
                {
                    description = string.Empty;
                }
                else
                {
                    description = value.Length > 250 ? value[..250] : value;
                }
            }
        }

        public UnitType UnitType { get; set; }

        public int AmountInStock { get; protected set; }

        public bool IsBelowThreshold { get; protected set; }

        public Price Price { get; set; }

        public Product(int id) : this(id, string.Empty)
        {
        }
        public Product(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public Product(int id, string name, string? description, Price price, UnitType unitType, int maxAmountInStock)
        {
            Id = id;
            Name = name;
            Description = description;
            Price = price;
            UnitType = unitType;
            maxItemsInStock = maxAmountInStock;

            UpdateLowStock();
        }

        public virtual void UseProduct(int item)
        {
            if (item <= AmountInStock)
            {
                AmountInStock -= item;
                UpdateLowStock();
                Log($"Amount in stock updated. Now {AmountInStock} items in stock.");
            }
            else
            {
                Log($"Not enough items on stock for {CreateSimpleProductRepresentation()}.{AmountInStock} in stock but {item}" +
                    $"requested.");
            }
        }

        //public virtual void IncreaseStock()
        //{
        //    AmountInStock++;
        //}
        public abstract void IncreaseStock();
        public virtual void IncreaseStock(int amount)
        {
            int newStock = AmountInStock + amount;
            if (newStock <= maxItemsInStock)
            {
                AmountInStock += amount;
            }
            else
            {
                AmountInStock = maxItemsInStock;// we only store possible items,overstock is not stored
                Log($"{CreateSimpleProductRepresentation()} stock overflow.{newStock - AmountInStock} items ordered that couldn't" +
                    $"be stored");
            }
            if (AmountInStock > StockThreshold)
            {
                IsBelowThreshold = false;
            }
        }


        protected virtual void DecreaseStock(int items, string reason)
        {
            if (items <= AmountInStock)
            {
                AmountInStock -= items;
            }
            else
            {
                AmountInStock = 0;
            }

            UpdateLowStock();

            Log(reason);
        }

        public string DisplayDetailsShort()
        {
            return $"{id}.{name}\n {AmountInStock} items in stock. ";
        }

        public virtual string DisplayFullDetails()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"{id}.{name}\n {description}\n {Price}\n{AmountInStock} items in stock.");
            if (IsBelowThreshold)
            {
                sb.Append("\n !!STOCK LOW!!");
            }
            return sb.ToString();
            // return DisplayFullDetails("");
        }

        public string DisplayFullDetails(string extraDetails)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"{id}.{name}\n {description}\n {Price}\n{AmountInStock} items in stock.");
            sb.Append(extraDetails);
            if (IsBelowThreshold)
            {
                sb.Append("\n !!STOCK LOW!!");
            }
            return sb.ToString();
        }

        public abstract object Clone();
    }
}
