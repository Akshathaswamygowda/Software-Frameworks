using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cecs575Assignment1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Input Set A");
            IntegerSet set1 = inputset();
            Console.WriteLine("Input Set B");
            IntegerSet set2 = inputset();
            Console.WriteLine("\nSet A contains elements:");
            Console.WriteLine(set1.ToString());
            Console.WriteLine("\nSet B contains elements:");
            Console.WriteLine(set2.ToString());
            if (set1.IsEqualTo(set2))
            {
                Console.WriteLine("Set A is equal to Set B");
            }
            else
                Console.WriteLine("Set A is not equal to B");

            IntegerSet union = set1.Union(set2);
            Console.WriteLine("Union of set A and set B contains");
            Console.WriteLine(union.ToString());

            IntegerSet intersection = set1.Intersection(set2);
            Console.WriteLine("Intersection of set A and set B contains");
            Console.WriteLine(intersection.ToString());
            int[] intarray = { 25, 67, 2, 9, 99, 105, 45, -5, 100, 1 };
            IntegerSet set3 = new IntegerSet(intarray);
            Console.WriteLine("New set contains");
            Console.WriteLine(set3.ToString());

            Console.WriteLine("Insert Element 77");
            set3.Insertset(77);
            Console.WriteLine("New Set Contains");
            Console.WriteLine(set3.ToString());

            Console.WriteLine("Delete Element 77");
            set3.Deleteset(77);
            Console.WriteLine("New Set Contains");
            Console.WriteLine(set3.ToString());
            Console.ReadLine();

        }
        

        /// <summary>
        /// Method for taking input from user
        /// </summary>
        /// <returns>returns class object</returns>
        private static IntegerSet inputset()
        {

            Char ch;
            IntegerSet set1 =new IntegerSet(); //Instance of class
            List<int> lis = new List<int>();   //for storing input values 

             //for taking input
            do                                           //Loop for continuing 
            {
                Console.WriteLine("Enter the number to Set");
                int A = Convert.ToInt32(Console.ReadLine());
                if (A > 0 && A < 101)                   //to check the input is withini valid range
                {
                    lis.Add(A);                        //Adding input to the list 
                    set1 = new IntegerSet(lis.ToArray());//Assigning list elements to the class instance
                    
                }
                //statement for asking to add another element into set
                Console.WriteLine("Do you want to add element (Y/N)?");
                ch = Convert.ToChar(Console.ReadLine().ToUpper());

            } while (ch.Equals(Char.Parse("Y")));   //condition for continuing the loop
            return set1;
        }
    }
}
