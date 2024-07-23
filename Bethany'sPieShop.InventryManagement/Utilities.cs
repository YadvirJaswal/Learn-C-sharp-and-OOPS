

using Bethany_sPieShop.InventoryManagement.Domain.Contract;
using Bethany_sPieShop.InventoryManagement.Domain.General;
using Bethany_sPieShop.InventoryManagement.Domain.OrderManagement;
using Bethany_sPieShop.InventoryManagement.Domain.ProductManagement;


namespace Bethany_sPieShop.InventoryManagement
{
    internal class Utilities
    {
        private static List<Product> inventory = new List<Product>();
        private static List<Order> orders = new List<Order>();

        internal static void IntializeStock()
        {
            //BoxedProduct bp = new BoxedProduct(6, "Eggs", "Lorem ipsum", new Price(){ ItemPrice = 10, Currency = Currency.Euro },
            //    6, 100);

            //bp.IncreaseStock(100);
            //bp.UseProduct(10);
            RegularProduct rp = new RegularProduct(7, "Pie candles", "Lorem ipsum", new Price() { ItemPrice = 10,
                Currency = Currency.Euro}, UnitType.PerItem,100);

            double value = rp.ConvertProductPrice(Currency.Dollar);

            ProductRepository productRepository = new ProductRepository();
            inventory = productRepository.LoadProductsFromFile();
            
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Loaded {inventory.Count} products!");
            Console.WriteLine("Press enter to continue");

            Console.ResetColor();
            Console.ReadLine();
        }

        internal static void ShowMainMenu()
        {
            Console.Clear();
            Console.WriteLine("****************");
            Console.WriteLine("Select an action");
            Console.WriteLine("****************");

            Console.WriteLine("1. Inventory management");
            Console.WriteLine("2. Order management");
            Console.WriteLine("3. Settings");
            Console.WriteLine("4. Save all data");
            Console.WriteLine("0. Close application");

            Console.Write("Your Selection: ");
            string? userSelection = Console.ReadLine();

            switch(userSelection)
            {
                case "1":
                    ShowInventoryManagementMenu();
                    break;
                case "2":
                    ShowOrderManagementMenu();
                    break;
                case "3":
                    ShowSettingMenu();
                    break;
                case "4":
                    SaveAllData();
                    break;
                case "0":
                    break;
                default:
                    Console.WriteLine("Select valid action!");
                    break;
            }
        }
        private static void SaveAllData()
        {
            ProductRepository productRepository = new ProductRepository();
            List<ISaveable> saveables = new List<ISaveable>();

            foreach (var item in inventory)
            {
                if(item is ISaveable)
                {
                    saveables.Add(item as ISaveable);
                }                
            }
            productRepository.SaveToFile(saveables);

            Console.ReadLine();
            ShowMainMenu();
        }

        private static void ShowInventoryManagementMenu()
        {
            Console.Clear();
            string? userSelection;
            do
            {
                Console.WriteLine("*************************");
                Console.WriteLine("* Inventory Management * ");
                Console.WriteLine("*************************");

                ShowAllProductOverview();

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("What do you want to do?");
                Console.ResetColor();

                Console.WriteLine("1. View details of product");
                Console.WriteLine("2. Add new product");
                Console.WriteLine("3. Clone product");
                Console.WriteLine("4. View product with low stock");
                Console.WriteLine("0. Back to main menu");

                Console.Write("Your Selection: ");
                userSelection = Console.ReadLine();
                switch (userSelection)
                {
                    case"1":
                        ShowDetailsAndUseProduct();
                        break;
                    case "2":
                        ShowCreateNewProduct();
                        break;
                    case "3":
                        ShowCloneExistingProduct();
                        break;
                    case "4":
                        ShowProductsLowOnStock();
                        break;
                }
            } while (userSelection!="0");
            ShowMainMenu();
        }
        private static void ShowAllProductOverview()
        {
            foreach(var product in inventory)
            {
                Console.WriteLine(product.DisplayDetailsShort());
                Console.WriteLine();
            }
        }
        

        private static void ShowDetailsAndUseProduct()
        {
            string? userSelection = String.Empty;
           
            Console.Write("Enter the Id of the product: ");
            string? selectedProductId = Console.ReadLine();

            if (selectedProductId != null)
            {
                Product? selectedProduct = inventory.Where(product => product.Id == int.Parse
                (selectedProductId)).FirstOrDefault();

                if(selectedProduct != null)
                {
                    Console.WriteLine(selectedProduct.DisplayFullDetails());

                    Console.WriteLine("\nWhat do you want to do:- ");
                    Console.WriteLine("1. Use product");
                    Console.WriteLine("0. Back to inventory overview");

                    Console.Write("Your Selection: ");
                    userSelection= Console.ReadLine();

                    if(userSelection == "1")
                    {
                        Console.WriteLine("How many products do you want to use?");
                        int amount = int.Parse(Console.ReadLine() ?? "0");

                        selectedProduct.UseProduct(amount);
                        Console.ReadLine();
                    }
                }
            }
            else
            {
                Console.WriteLine("Non-existing product selected.Please try again.");
            }
        }

        private static void ShowProductsLowOnStock()
        {
            List<Product> lowOnStockProducts = inventory.Where(p => p.IsBelowThreshold).ToList();

            if(lowOnStockProducts.Count > 0)
            {
                Console.WriteLine("The following items are low on stock.Order more soon!");

                foreach(var product in lowOnStockProducts)
                {
                    Console.WriteLine(product.DisplayDetailsShort());
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("No items are currently low on stock.");
            }
            Console.ReadLine() ;
        }

        private static void ShowOrderManagementMenu()
        {
            string? userSelection = string.Empty;
            do
            {
                Console.ResetColor();
                Console.Clear();

                Console.WriteLine("**********************");
                Console.WriteLine("* Order Management * ");
                Console.WriteLine("***********************");

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("What do you want to do?");
                Console.ResetColor();

                Console.WriteLine("1. Open order overview");
                Console.WriteLine("2. Add new order");
                Console.WriteLine("0. Back to main menu");

                Console.Write("Your selection: ");
                userSelection = Console.ReadLine();

                switch (userSelection)
                {
                    case "1":
                        ShowOpenOrderOverview();
                        break;
                    case "2":
                        ShowAddNewOrder();
                        break;
                    default:
                        Console.WriteLine("Invalid selection. Please try again.");
                        break;
                }
            } while (userSelection!="0");
            ShowMainMenu();
        }
        private static void ShowOpenOrderOverview()
        {
            ShowFulfilledOrders();
            if(orders.Count > 0)
            {
                Console.WriteLine("Open orders :");
                foreach(var order in orders)
                {
                    Console.WriteLine(order.ShowOrderDetails());
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("There are no open orders.");
            }
            Console.ReadLine();
        }
        private static void ShowFulfilledOrders()
        {
            Console.WriteLine("Checking fulfilled orders.");
            foreach (var order in orders)
            {
                if(!order.IsOrderFullfilment && order.OrderFullfilmentDate < DateTime.Now)
                {
                    foreach(var orderItem in order.OrderItems)
                    {
                        Product? selectedProduct = inventory.Where(p => p.Id ==  orderItem.ProductId).FirstOrDefault();
                        if(selectedProduct != null)
                        {
                            selectedProduct.IncreaseStock(orderItem.AmountOrdered);
                        }
                        order.IsOrderFullfilment = true;
                    }
                }
            }
            orders.RemoveAll(o => o.IsOrderFullfilment);
            Console.WriteLine("Fullfilled orders checked.");
        }

        private static void ShowAddNewOrder()
        {
            Order newOrder = new Order();
            string? selectedProductId = string.Empty;

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Creating new order!");
            Console.ResetColor();

            do
            {
                ShowAllProductOverview();

                Console.WriteLine("Which product do you want to order? enter 0 to stop adding new products to the order!");
                Console.Write("Enter the Id of the product: ");
                selectedProductId = Console.ReadLine();

                if(selectedProductId != "0")
                {
                    Product? selectedProduct = inventory.Where(p => p.Id == int.Parse(selectedProductId)).FirstOrDefault();

                    if(selectedProduct != null)
                    {
                        Console.Write("How many do you want to order: ");
                        int amountOrdered = int.Parse(Console.ReadLine()  ?? "0");

                        OrderItem orderItem = new OrderItem()
                        {
                            ProductId = selectedProduct.Id,
                            ProductName = selectedProduct.Name,
                            AmountOrdered = amountOrdered
                        };
                        newOrder.OrderItems.Add(orderItem);
                    }   
                }
            }while(selectedProductId != "0");

            Console.WriteLine("creating orders............");
            orders.Add(newOrder);
            Console.WriteLine("Order now created!");
            Console.ReadLine();
        }

        private static void ShowSettingMenu()
        {
            string? userSelection;

            do
            {
                Console.ResetColor();
                Console.Clear();
                Console.WriteLine("********");
                Console.WriteLine("Settings");
                Console.WriteLine("********");

                Console.WriteLine("What do you want to do?");
                Console.WriteLine("1. Change stock threshold");
                Console.WriteLine("0. Back to main menu");

                Console.Write("Your selection: ");
                userSelection = Console.ReadLine();

                switch (userSelection)
                {
                    case "1":
                        ShowChangeStockThreshold();
                        break;
                    default:
                        Console.WriteLine("Invalid action.Please try again!");
                        break;
                }
            } while (userSelection != "0");
            ShowMainMenu();
        }

        private static void ShowChangeStockThreshold()
        {
            Console.WriteLine($"Enter the new stock threshold ( Current value: {Product.StockThreshold}).This applies to all products!");
            Console.Write("New value: ");
            int newValue = int.Parse( Console.ReadLine() ?? "0");
            Product.StockThreshold = newValue;
            Console.WriteLine($"New stock threshold set to {Product.StockThreshold}");

            foreach(var product in inventory)
            {
                product.UpdateLowStock();
            }
            Console.ReadLine();
        }

        private static void ShowCreateNewProduct()
        {
            UnitType unitType = UnitType.PerItem;//default

            Console.WriteLine("What product do you want to create?");
            Console.WriteLine("1.Regular product \n 2.Bulk product \n 3.Fresh product \n 4.Boxed product");

            Console.Write("Your Selection: ");
            var productType = Console.ReadLine();

            if(productType != "1" && productType != "2" && productType != "3" && productType != "4")
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid selection!!!!");
                Console.ResetColor();
            }

            Product? newProduct = null;

            Console.Write("Enter the name of the product: ");
            string name = Console.ReadLine() ?? string.Empty;

            Console.Write("Enter the price of the product: ");
            double price = double.Parse( Console.ReadLine() ?? "0.0");

            ShowAllCurrencies();
            Console.Write("Select the currency: ");
            Currency currency = (Currency)Enum.Parse(typeof(Currency), Console.ReadLine() ?? "1");

            Console.Write("Enter the description of the product: ");
            string description = Console.ReadLine() ?? string.Empty;

            if(productType == "1")
            {
                ShowAllUnitTypes();
                Console.Write("Select the unit type: ");
                unitType = (UnitType)Enum.Parse(typeof(UnitType), Console.ReadLine() ?? "1");
            }

            Console.Write("Enter the maximum number of item in stock for this product: ");
            int maxInStock = int.Parse(Console.ReadLine() ?? "0");

            int newId = inventory.Max(p => p.Id)+1; //find higest id and increase with 1

            switch (productType)
            {
                case "1":
                    newProduct = new RegularProduct(newId, name, description, new Price() { ItemPrice = price, Currency = currency }
                    , unitType,maxInStock);
                    break;
                case "2":
                    newProduct = new BulkProduct(newId++ , name , description , new Price() { ItemPrice = price ,
                        Currency = currency}, maxInStock);
                    break;
                case"3":
                    newProduct = new FreshProduct(newId++ , name , description, new Price() { ItemPrice = price ,
                        Currency = currency},unitType, maxInStock);
                    break;
                case "4":
                    Console.Write("Enter the number of items per box: ");
                    int numberInBox = int.Parse(Console.ReadLine() ?? "0");

                    newProduct = new BoxedProduct(newId++ , name , description , new Price() { ItemPrice = price ,
                        Currency = currency}, maxInStock,numberInBox);
                    break;
            }
            if(newProduct != null)
            {
                inventory.Add(newProduct);
            }
        }

        private static void ShowAllUnitTypes()
        {
            int i = 1;
            foreach(string name in Enum.GetNames(typeof(UnitType)))
            {
                Console.WriteLine($"{i}.{name}");
                i++;
            }
        }
        private static void ShowAllCurrencies()
        {
            int i = 1;
            foreach(string name in Enum.GetNames(typeof(Currency)))
            {
                Console.WriteLine($"{i}.{name}");
                i++;
            }
        }

        private static void ShowCloneExistingProduct()
        {
            string? newId = string.Empty;

            Console.Write("Enter the Id of the product to clone : ");
            string? selectedProductId = Console.ReadLine();

            if(selectedProductId != null )
            {
                Product? selectedProduct = inventory.Where(p => p.Id == int.Parse(selectedProductId)).FirstOrDefault();

                if(selectedProduct != null )
                {
                    Console.Write("Enter the new Id of the cloned product : ");
                    newId = Console.ReadLine();

                    Product? p = selectedProduct.Clone() as Product;

                    if(p != null )
                    {
                        p.Id = int.Parse(newId);
                        inventory.Add(p);
                    }
                }
            }
            else
            {
                Console.WriteLine("Non-existing product selected. Please try again.");
            }
        }
    }
}
