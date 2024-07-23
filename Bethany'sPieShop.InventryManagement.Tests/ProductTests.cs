using Bethany_sPieShop.InventoryManagement.Domain.General;
using Bethany_sPieShop.InventoryManagement.Domain.ProductManagement;

namespace Bethany_sPieShop.InventoryManagement.Tests
{
    public class ProductTests
    {
        [Fact]
        public void UseProduct_Reduces_AmountInStock()
        {
            //Arrange

            RegularProduct product = new RegularProduct(1,"Sugar" , "Lorem ipsum", new Price() { ItemPrice = 10, Currency = Currency.Euro}, 
                UnitType.PerKg,100);
            product.IncreaseStock(100);

            // Act
            product.UseProduct(20);

            //Assert

            Assert.Equal(80,product.AmountInStock);
        }

        [Fact]
        public void UseProduct_ItemsHigherThanStock_NoChangeToStock()
        {
            //Arrange
            RegularProduct product = new RegularProduct(1, "Sugar", "Lorem ipsum", new Price() { ItemPrice = 10, Currency = Currency.Euro },
                UnitType.PerKg, 100);
            product.IncreaseStock(10);

            //Act
            product.UseProduct(20);

            //Assert
            Assert.Equal(10,product.AmountInStock);
        }
        [Fact]  
        public void UseProduct_Reduces_AmountInStock_StockBelowThreshold()
        {
            RegularProduct product = new RegularProduct(1, "Sugar", "Lorem ipsum", new Price() { ItemPrice = 10, Currency = Currency.Euro },
                UnitType.PerKg, 100);

            int increseValue = 100;
            product.IncreaseStock(increseValue);

            product.UseProduct(increseValue - 1);

            Assert.True(product.IsBelowThreshold);
        }

        [Fact]
        public void IncreaseStock_AddOne()
        {
            RegularProduct product = new RegularProduct(1, "Sugar", "Lorem ipsum", new Price() { ItemPrice = 10, Currency = Currency.Euro },
                UnitType.PerKg, 100);

            product.IncreaseStock();

            Assert.Equal(1,product.AmountInStock);
        }

        [Fact]

        public void IncreaseStock_PassedInValue_BelowMaxAmount()
        {
            RegularProduct product = new RegularProduct(1, "Sugar", "Lorem ipsum", new Price() { ItemPrice = 10, Currency = Currency.Euro },
                UnitType.PerKg, 100);

            product.IncreaseStock(20);

            Assert.Equal(20,product.AmountInStock);
        }

        [Fact]
        public void IncreaseStock_PassedInValue_AboveMaxAmount()
        {
            RegularProduct product = new RegularProduct(1, "Sugar", "Lorem ipsum", new Price() { ItemPrice = 10, Currency = Currency.Euro },
               UnitType.PerKg, 100);

            product.IncreaseStock(200);

            Assert.Equal(100,product.AmountInStock);
        }
    }
}