using Bethany_sPieShop.InventoryManagement.Domain.Contract;
using Bethany_sPieShop.InventoryManagement.Domain.General;
using Bethany_sPieShop.InventoryManagement.Domain.ProductManagement;
using System.Text;


namespace Bethany_sPieShop.InventoryManagement
{
    internal class ProductRepository
    {
        string directory = @"C:\data\BethanysPieShopHRM1\" ;
        string fileName = "products.txt";

        private void CheckingForExistingFile()
        {
            string path = $"{directory} {fileName}";
            
            bool isFileExists = File.Exists(path) ;
            if (!isFileExists )
            {
                //create directory
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(directory);
                }
                using FileStream fileStream = File.Create(path);
            }
        }
        public List<Product> LoadProductsFromFile()
        {
            List<Product> products = new List<Product>();
            string path = $"{directory} {fileName}";

            try
            {
                CheckingForExistingFile();

                string[] productAsString = File.ReadAllLines(path);

                for (int i = 0; i < productAsString.Length; i++)
                {
                    string[] productSplits = productAsString[i].Split(";");

                    bool success = int.TryParse(productSplits[0], out int productId);
                    if (!success)
                    {
                        productId = 0;
                    }
                    string name = productSplits[1];
                    string description = productSplits[2];

                    success = int.TryParse(productSplits[3], out int maxItemsInStock);
                    if (!success)
                    {
                        maxItemsInStock = 100; //default value
                    }

                    success = int.TryParse(productSplits[4], out int itemPrice);
                    if (!success)
                    {
                        itemPrice = 0;  // default value
                    }

                    success = Enum.TryParse(productSplits[5], out Currency currency);
                    if (!success)
                    {
                        currency = Currency.Dollar; // default value
                    }

                    success = Enum.TryParse(productSplits[6], out UnitType unitType);
                    if (!success)
                    {
                        unitType = UnitType.PerItem;
                    }
                    string productType = productSplits[7];
                    Product product = null;

                    switch (productType)
                    {
                        case "1":
                            success = int.TryParse(productSplits[8], out int amountPerBox);
                            if (!success)
                            {
                                amountPerBox = 0;
                            }

                            product = new BoxedProduct(productId, name, description, new Price() {ItemPrice = itemPrice, 
                                Currency = currency}, maxItemsInStock,amountPerBox);
                            break;

                        case "2":
                            product = new FreshProduct(productId , name , description , new Price() { ItemPrice = itemPrice,
                            Currency = currency},unitType,maxItemsInStock);
                            break;

                        case "3":
                            product = new BulkProduct(productId, name , description , new Price() { ItemPrice = itemPrice , 
                            Currency = currency},maxItemsInStock);
                            break;
                        case "4":
                            product = new RegularProduct(productId, name, description, new Price() { ItemPrice = itemPrice , Currency = currency
                            },unitType,maxItemsInStock);
                            break;
                    }

                    //Product product = new Product(productId, name, description, new Price()
                    //{
                    //    ItemPrice = itemPrice,
                    //    Currency = currency
                    //}, unitType, maxItemsInStock);

                    products.Add(product);
                }
            }
            catch(IndexOutOfRangeException iex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Something went wrong parsing the file, please check the data!");
                Console.WriteLine(iex.Message);
            }
            catch(FileNotFoundException fex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("The file couldn't be found!");
                Console.WriteLine(fex.Message);
                Console.WriteLine(fex.StackTrace);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Something went wrong while loading the file!");
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.ResetColor();
            }
            return products;
        }
        public void SaveToFile(List<ISaveable> saveables)
        {
            StringBuilder sb = new StringBuilder();
            string path = $"{directory} {fileName}";

            foreach (var item in saveables)
            {
                sb.Append(item.ConvertToStringForSaving());
                sb.Append(Environment.NewLine);
            }
            File.WriteAllText(path, sb.ToString());

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Saved items successfully !!!");
            Console.ResetColor();
        }
    }
    
}
