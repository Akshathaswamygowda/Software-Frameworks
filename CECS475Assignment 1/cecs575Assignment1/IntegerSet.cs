using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cecs575Assignment1
{ 
    //IntegerSet class for performing set operation
    class IntegerSet
    {
        /// <summary>
        /// Each Integer Set is represented by boolean array .</summary>
        /// 
        public static int size = 101;
        private bool[] array=new bool[size] ;

        /// <summary>
        /// class  constructor. </summary>

        public IntegerSet()                                                   
        {
            
            array.DefaultIfEmpty<bool>();                                    

        }
        /// <summary>
        /// Parameter Constructor
        /// </summary>
        /// <param name="intarray"></param>
        public IntegerSet(int[] intarray)                                   
        {
            for (int i = 0; i < intarray.Length; i++)                     // Intializes the interger set to boolen array
            {
                if (intarray[i] >= 0 && intarray[i] <= 101)
                {
                    array[intarray[i]] = true;                           //Setting boolean array elements true based on integer set values 
                }
            }
        }

        /// <summary>
        /// Method for inserting num into set
        /// </summary>
        /// <param name="num"></param>
        public void Insertset(int num)                                 
        {
            if (num > 0 && num <= 101)
            {
                this.array[num] = true;                                 //sets the boolean array element to true
            }
        }

        /// <summary>
        /// Method for deleting num from set
        /// </summary>
        /// <param name="num"></param>
        public void Deleteset(int num)                                  
        {
            if (num > 0 && num <= 101)
            {
                this.array[num] = false;                                //sets the boolean array element to false
            }
        }

        /// <summary>
        /// Method for displaying the set elements
        /// </summary>
        /// <returns>returns string value</returns>
        public string ToString()                                        
        {
            string display="";
           
            for(int i=0;i<array.Length;i++)
            {
               
                if (array[i] == true)                                    //checks whichever array element is true  
                {
                    display = display + "  " + i.ToString();             //array elements with true value will added to the string   
                }
            }
            if (display == "")                                           //Checks if the set is empty
            {
                Console.WriteLine("---");
            }
            return display;                                         //return string elements
        }

        /// <summary>
        /// Method for comapring two sets to check if its equal
        /// </summary>
        /// <param name="ob"></param>
        /// <returns>return bool value</returns>
        public bool IsEqualTo(IntegerSet ob)                            
        {
            for(int i = 0; i < size; i++)
            {
                if (array[i] != ob.array[i])                             //two set objects
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// Method for union operation
        /// </summary>
        /// <param name="ob"></param>
        /// <returns>returns class object</returns>
        public IntegerSet Union(IntegerSet ob)                          
        {
            IntegerSet set4 = new IntegerSet();                         //creates new instance for displaying union of sets
            bool[] union = new bool[size];
            for(int i=0;i<size;i++)
            {
                
                if(array[i] || ob.array[i])                            
                {
                    union[i] = true;
                }
            }
            set4.array = union;
            return set4;
        }

        /// <summary>
        /// Method for intersection operation
        /// </summary>
        /// <param name="ob"></param>
        /// <returns>returns class object value</returns>
        public IntegerSet Intersection(IntegerSet ob)                   
        {
            IntegerSet set5 = new IntegerSet();                        //creates new instance for displaying intersection of sets
            bool[] inter = new bool[size];
            for(int i=0;i<size;i++)
            {
                if(array[i] && ob.array[i])                          
                {
                    inter[i] = true;
                }
            }
            set5.array = inter;
            return set5;
        }
    }
}
