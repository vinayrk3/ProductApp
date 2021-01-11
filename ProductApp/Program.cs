using System;
using System.IO;
using System.Collections.Generic;

namespace ProductApp
{
    class Program
    {
        public static string dirPath = @"D:\Testfile\";
        public static string filePath = dirPath + "productfile.txt";
        static void Main(string[] args)
        {
            Console.WriteLine("Product Inventory!");
            Product objProduct;
            List<Product> listProduct = new List<Product>();
            string ProductID;
            string ProductName;
            double ProductPrice;
            DateTime ManufactureDate, ExpiryDate;
            string choice;
            while (true)
            {
                Console.WriteLine("Enter the Product Details");
                Console.WriteLine("Press 1 to Add Product Details");
                Console.WriteLine("Press 2 to Display the List of products");
                choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        bool dirStatus = CheckDirectoryExists(dirPath);
                        if(dirStatus == true)
                        {
                            while (true)
                            {
                                Console.WriteLine("Enter the Product ID");
                                ProductID = Console.ReadLine();
                                Console.WriteLine("Enter the Product Name");
                                ProductName = Console.ReadLine();
                                Console.WriteLine("Enter the Product Price");
                                ProductPrice = Convert.ToInt32(Console.ReadLine());
                                Console.WriteLine("Enter the Manufacturing Date");
                                ManufactureDate = Convert.ToDateTime(Console.ReadLine());
                                Console.WriteLine("Enter the Expiry Date");
                                ExpiryDate = Convert.ToDateTime(Console.ReadLine());

                                objProduct = new Product(ProductID, ProductName, ProductPrice, ManufactureDate, ExpiryDate);
                                listProduct.Add(objProduct);

                                Console.WriteLine("Do you want to add another Record?");
                                Console.WriteLine("Press Y for Yes or N for No");
                                var answer = Console.ReadLine();
                                if (answer.ToUpper().Equals("Y") == true)
                                    continue;
                                else
                                    break;
                            }
                            WriteData(listProduct);
                            listProduct.Clear();
                            
                        }
                        else
                        {
                            Console.WriteLine("Directory Not Found");
                        }
                        break;

                    case "2":
                        List<Product> prodItems = ReadData();
                        if (prodItems.Count == 0)
                            Console.WriteLine("Product list is Empty");
                        else
                        {
                            String data = String.Format("{0,-20} {1,-20} {2,-20} {3, -20} {4, -20} \n", "Product Id", "Product Name", "Price", "Manufacturing Date", "Expiry Date");
                            Console.WriteLine();
                            Console.WriteLine("*********Product List*******");
                            foreach (var item in prodItems)
                            {
                                data += String.Format("{0,-20} {1,-20} {2, -20} {3, -20} {4, -20} \n", item.productId, item.productName, item.producPrice, item.manuDate.ToString("dd/MM/yyyy"), item.expDate.ToString("dd/MM/yyyy"));
                            }
                            Console.WriteLine($"\n{data}");
                        }
                        break;
                    default:
                        Console.WriteLine("Invalid Choice");
                        break;
                        
                }

                Console.Write("Do you want to continue(Y/N)?:");
                var response = Console.ReadLine();

                if (response.ToUpper().Equals("Y") == true)
                    continue;
                else
                    break;
            }
        }

        static void WriteData(List<Product> ProdList)
        {
            FileStream fs = null;
            StreamWriter sw = null;

            try
            {
                fs = new FileStream(filePath, FileMode.Append, FileAccess.Write);
                sw = new StreamWriter(fs);
                foreach(Product item in ProdList)
                {
                    sw.Write(item.productId + "|");
                    sw.Write(item.productName + "|");
                    sw.Write(item.producPrice + "|");
                    sw.Write(item.manuDate + "|");
                    sw.WriteLine(item.expDate + "|");
                }
            }

            catch (DirectoryNotFoundException)
            {
                Console.WriteLine($"Directory Path {dirPath} does not exist");
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"File path {filePath} does not exist");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Error {ex.Message}");
            }
            finally
            {
                if (fs != null)
                {
                    sw.Close();
                    fs.Close();
                }
            }
        }

        static List<Product> ReadData()
        {
            FileStream fs = null;
            StreamReader sr = null;
            Product objProduct;
            List<Product> prodList = new List<Product>();

            try
            {
                if(Directory.Exists(dirPath) && File.Exists(filePath))
                {
                    fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                    sr = new StreamReader(fs);
                    while(sr.Peek()!= -1)
                    {
                        string record = sr.ReadLine();
                        string[] columns = record.Split("|");
                        objProduct = new Product(columns[0], columns[1], Convert.ToDouble(columns[2]), Convert.ToDateTime(columns[3]), Convert.ToDateTime(columns[4]));
                        prodList.Add(objProduct);

                    }
                }
                else
                {
                    Console.WriteLine("Directory or filepath does not exist");
                }
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine($"Directory path {dirPath} does not exist");
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"File path {filePath} does not exist");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Error  {ex.Message}");
            }
            finally
            {
                if (fs != null)
                {
                    sr.Close();
                    fs.Close();
                }
            }
            return prodList;
        }

        static bool CheckDirectoryExists(string path)
        {
            bool flag;
            if (Directory.Exists(path))
            {
                flag = true;
            }
            else
            {
                flag = false;
            }
            return flag;
        }

        class Product
        {
            public string productId;
            public string productName;
            public double producPrice;
            public DateTime manuDate, expDate;

            public Product(string ProductID, string ProductName, double ProductPrice, DateTime ManufactureDate, DateTime ExpiryDate)
            {
                this.productId = ProductID;
                this.productName = ProductName;
                this.producPrice = ProductPrice;
                this.manuDate = ManufactureDate;
                this.expDate = ExpiryDate;
            }
        }
    }
}
