using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CECS475Assignment2
{

    /// <summary>
    /// Class maintainig the Stock details
    /// </summary>
    class Stock
    {
        // variable for Stock name
        public String stockName;

        // InitialValue of the Stock
        public int initialValue;

        // value of stock after few changes
        private int currentValue;

        //Maximum change
        public int maximumChange;

        //Threshold value
        private int threshold;

        // No of times the stock has changed
        private int numberchanges;

        //Random variable for changing stock value
        Random rnd;

        //Thread for synchornization
        Thread thread;

        private static object lock_obj = new object();


        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="name">Name of the Stock</param>
        /// <param name="stringValue">Current value</param>
        /// <param name="maxchange">Maximum number of change</param>
        /// <param name="notificationthreshold">Threshold value</param>
        public Stock(string name, int stringValue, int maxchange, int notificationthreshold)
        {
            stockName = name;
            initialValue = stringValue;
            currentValue = initialValue;
            maximumChange = maxchange;
            threshold = notificationthreshold;

            //for random number generator
            rnd = new Random();  

            //Parametrized thread for synchornize the stock
            thread = new Thread(new ParameterizedThreadStart(Activate));

            //to start the thread
            thread.Start(this);
        }

        /// <summary>
        /// Method for activate the thread 
        /// </summary>
        /// <param name="obj">which takes parameter as object </param>
        public static void Activate(Object obj)
        {
            //Loop for thread exceution
            for (int i = 0; i < 10; i++)
            {
                //Lock for synchornization of the thread
                lock (lock_obj)
                {
                    Update(obj);

                    //delay of 500 miliseconds
                    Thread.Sleep(500);
                }
            }
        }

        /// <summary>
        /// Method for calculation of the Stock
        /// </summary>
        /// <param name="obj"></param>
        private static void Update(object obj)
        {
            //casting from object class
            Stock s = (Stock)obj;

            // randomly generated number should be updated here
            s.currentValue += s.rnd.Next(1, s.maximumChange);

            //update the numberchanges
            s.numberchanges++;
            
            //Absolute difference of currentvalue and initialvalue
            double difference=Math.Abs(s.currentValue - s.initialValue);
            if (difference > s.threshold)
            {
                //if the difference is greater than threshold then raise event
                StockNotificationArgs args = new StockNotificationArgs();
                args.Stockname = s.stockName;
                args.CurrentValue = s.currentValue;
                args.MaximumChange = s.maximumChange;
                s.Thresholdreached(args);
            }
        }

        /// <summary>
        /// Method for invoking the event
        /// </summary>
        /// <param name="e">Eventargs object</param>
        public void Thresholdreached(StockNotificationArgs e)
        {
            thresholdreached.Invoke(e.Stockname,e.CurrentValue,e.MaximumChange);
        }
       
        /// <summary>
        /// Action Delagate for the event
        /// </summary>
        public Action<string, int, int> thresholdreached;

    }

    /// <summary>
    /// class for event
    /// </summary>
    public class StockNotificationArgs:EventArgs
    {
        /// <summary>
        /// Proterties of the Stock Variable
        /// </summary>
        public string Stockname { get; set; }
        public int CurrentValue { get; set; }
        public int MaximumChange { get; set; }

    }
}
