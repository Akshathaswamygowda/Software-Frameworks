using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CECS475Assignment2
{
    class Program
    {
        
        static void Main(string[] args)
        {
            Stock stock1 = new Stock("Technology", 160, 5, 15);
            Stock stock2 = new Stock("Retail", 30, 2, 6);
            Stock stock3 = new Stock("Banking", 90, 4, 10);
            Stock stock4 = new Stock("Commodity", 500, 20, 50);

            Broker b1 = new Broker("Broker 1");
            b1.AddStock(stock1);
            b1.AddStock(stock2);

            Broker b2 = new Broker("Broker 2");
            b2.AddStock(stock1);
            b2.AddStock(stock3);
            b2.AddStock(stock4);

            Broker b3 = new Broker("Broker 3");
            b3.AddStock(stock1);
            b3.AddStock(stock3);

            Broker b4 = new Broker("Broker 4");
            b4.AddStock(stock1);
            b4.AddStock(stock2);
            b4.AddStock(stock3);
            b4.AddStock(stock4);
            if (File.Exists(@"file.txt"))
            {
                if (new FileInfo(@"file.txt").Length != 0)
                {
                    System.IO.File.WriteAllText(@"file.txt", string.Empty);
                }
            }
            
            Console.WriteLine("Broker".PadRight(10) + "\t\t" + "Stock".PadRight(10) + "\t\t\t" + "Value".PadRight(10) + "\t" + "Changes".PadRight(10));
            Console.ReadLine();
        }
    }
}
