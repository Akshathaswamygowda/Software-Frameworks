using System;
using GalaSoft.MvvmLight;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Input;
using CECSAssignment3.ViewModel;

namespace CECSAssignment3.Model
{
    
    public class MemberDB: ObservableObject
    {
        /// <summary>
        /// Indexer Reference
        /// </summary>
        private MemberIndexer MemberIndexer = new MemberIndexer(); 
        /// <summary>
        /// MemberList Constructor
        /// </summary>
        /// <param name="m"></param>
        public MemberDB(ObservableCollection<Member> m)
        {
            MemberIndexer.members = m;
        }

      /// <summary>
      /// Method For reteriving Data from File
      /// </summary>
      /// <returns></returns>
        public ObservableCollection<Member> GetMembership()
        {
            string row = "";
            string[] columns;
            try
            {
                if (File.Exists(@"file.txt"))
               {
                    StreamReader input = new StreamReader(new FileStream(@"file.txt",FileMode.OpenOrCreate,FileAccess.Read),false);
                    while(input.Peek()!=-1)
                    {
                        row = input.ReadLine();
                        columns = row.Split('-');
                        Member m = new Member(columns[0], columns[1], columns[2]);
                        Members.Add(m);

                    }
                    input.Close();
                    
                }
            }
            catch(FileNotFoundException)
            {
                Console.WriteLine("File not found");
            }
            catch(FormatException)
            {
                Console.WriteLine("Invalid e-mail address format");
            }
            return MemberIndexer.members;
        }
        /// <summary>
        /// Method For Saving event in the text file
        /// </summary>
        /// <param name="memberdata"></param>
        public void SaveMembership(ObservableCollection<Member> memberdata)
        {
           
            using (StreamWriter writer = new StreamWriter(@"file.txt", false))
            {
                foreach (Member m in memberdata)
                {
                    writer.Write(m.Firstname+"-");
                    writer.Write(m.Lastname+"-");
                    writer.Write(m.Email);
                    writer.WriteLine();
                    
                }
                writer.Close();
            }
            
        }
        /// <summary>
        /// Operator + overloaded Method
        /// </summary>
        /// <param name="m1"></param>
        /// <param name="m"></param>
        /// <returns></returns>
        public static MemberDB operator +(MemberDB m1, Member m)
        {
            m1.Add(m);
            return m1;
        }
        /// <summary>
        /// Add Member Method
        /// </summary>
        /// <param name="m"></param>
        public void Add(Member m) => MemberIndexer.members.Add(m);
        /// <summary>
        /// Remove Member Method
        /// </summary>
        /// <param name="m"></param>
        public void Remove(Member m) => MemberIndexer.members.Remove(m);
        /// <summary>
        /// Operator - overloaded method
        /// </summary>
        /// <param name="m1"></param>
        /// <param name="m"></param>
        /// <returns></returns>
        public static MemberDB operator -(MemberDB m1, Member m)
        {
            m1.Remove(m);
            return m1;
        }
     /// <summary>
     /// Property of ObservableCollection
     /// </summary>
        public ObservableCollection<Member> Members
        {
            get
            {
                return MemberIndexer.members;
            }
            set
            {
                MemberIndexer.members = value;
                RaisePropertyChanged("Member");
              
                
            }
        }
        /// <summary>
        /// Method For the Event
        /// </summary>
        /// <param name="e"></param>
        public void OnChanged(ChangeHandlerEventArgs e)
        {
            AddViewModel av = new AddViewModel();
            Changed = new Action<ObservableCollection<Member>>(av.Event_OnChanged);
            Changed.Invoke(e.MemberList);
        }
        /// <summary>
        /// Delegate For the Event
        /// </summary>
        public Action<ObservableCollection<Member>> Changed;
    }
    /// <summary>
    /// Class for Indexer
    /// </summary>
    class MemberIndexer
    {
        public ObservableCollection<Member> members;
        /// <summary>
        /// Indexer Property
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public Member this[int i]
        {
            get
            {
                return members[i];
            }
            set
            {
                members[i] = value;
            }
        }

    }
    /// <summary>
    /// Class for Event with observablecollection property
    /// </summary>
   public class ChangeHandlerEventArgs:EventArgs
    {
        public ObservableCollection<Member> MemberList { get; set; }
    }
}
