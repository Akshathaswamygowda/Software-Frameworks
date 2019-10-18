using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CECS475Assignment2
{

    /// <summary>
    /// Class Broker
    /// </summary>
    class Broker
    {
        
        /// <summary>
        /// Variable for broker name
        /// </summary>
        public string brokerName;

        /// <summary>
        /// List for storing all the stock
        /// </summary>
        List<Stock> list=new List<Stock>();

        /// <summary>
        /// Dictionary for retrieving the initial value from stock
        /// </summary>
        Dictionary<string, int> stocknames = new Dictionary<string, int>();

        /// <summary>
        /// Class borker constructor
        /// </summary>
        /// <param name="brokername">Assign the brokername</param>
        public Broker(string brokername)
        {
            brokerName = brokername;
        }


        /// <summary>
        /// Method for adding stock object into list
        /// </summary>
        /// <param name="s">Stock object</param>
        public void AddStock(Stock s)
        {
            stocknames.Add(s.stockName,s.initialValue);
            list.Add(s);
            s.thresholdreached += S_thresholdreached;
        }

        /// <summary>
        /// Method for printing all the values in console and in file
        /// </summary>
        /// <param name="name">name of the stock</param>
        /// <param name="currentvalue">currentvalue of the stock</param>
        /// <param name="maximumchange">maximum change of the stock</param>
        private void S_thresholdreached(string name, int currentvalue, int maximumchange)
        {
            Program p = new Program();
            //Printing in console window
            Console.WriteLine(brokerName.PadRight(10) + "\t\t" + name.PadRight(20) + "\t\t " + currentvalue + "\t\t " + maximumchange);

            //Writing in File
            if (new FileInfo(@"file.txt").Length == 0)
            {
                using (StreamWriter writer = new StreamWriter(@"file.txt",true))
                {
                    writer.WriteLine("Date and Time".PadRight(20) + "\t\t" + "Stock".PadRight(10) + "\t" + "Intial Value" + "\t" + "Current Value".PadRight(10));
                }
            }

            //File is stored in /bin/Debug folder
            using (StreamWriter writer = File.AppendText(@"file.txt"))
            {
                //Date and time object
                DateTime dt = DateTime.Now;

                //data time standard
                string culturename = "en-US";
                var culture = new CultureInfo(culturename);
                //Writing in file
                writer.WriteLine(dt.ToString(culture).PadRight(10) + "\t\t" + name.PadRight(10) + "\t\t" + stocknames[name] + "\t\t" + currentvalue);

            }
          
        }
        
    }
}
