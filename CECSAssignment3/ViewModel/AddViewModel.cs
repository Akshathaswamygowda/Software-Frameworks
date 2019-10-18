using CECSAssignment3.Message;
using CECSAssignment3.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CECSAssignment3.ViewModel
{
    
    public class AddViewModel:ViewModelBase
    {
        
        private ObservableCollection<Member> members;
        Member m;
        MessageMember message;
        private MemberDB database;
        private string enteredFName;
        private string enteredLName;
        private string enteredEmailId;
        private int text_limit = 25;

        /// <summary>
        /// Command Binding for SaveButton
        /// </summary>
        public ICommand SaveCommand { get; private set; }

        /// <summary>
        /// Command Binding for Cancel Button
        /// </summary>
        public ICommand CancelCommand { get; private set; }

        /// <summary>
        /// AddViewModel Constructor
        /// </summary>
        public AddViewModel()
        {
            message = new MessageMember();
            m = new Member(EnteredFName, enteredLName, EnteredEmailId);
            members = new ObservableCollection<Member>();
            database = new MemberDB(members);
            SaveCommand = new RelayCommand<Window>(SaveMethod);
            CancelCommand = new RelayCommand<ICloseable>(CancelMethod);
            database.Changed += Event_OnChanged;
        }

        /// <summary>
        /// Cancel Method Operation
        /// </summary>
        /// <param name="window">for closing window</param>
        private void CancelMethod(ICloseable window)
        {
            if(window!=null)
            {
                window.Close();
            }
        }
       
        /// <summary>
        /// Method For Saving button operation
        /// </summary>
        /// <param name="window"></param>
        public void SaveMethod(Window window)
        {
            if (window != null)
            {
                if (EnteredFName != null || EnteredLName != null || EnteredEmailId != null)
                {
                    ChangeHandlerEventArgs args = new ChangeHandlerEventArgs();
                    m = new Member(EnteredFName, EnteredLName, EnteredEmailId);
                    members.Add(m);
                    args.MemberList = members;
                    MemberDB memberdb = new MemberDB(members);
                    memberdb.OnChanged(args);
                    window.Close();
                }
                else
                {
                    MessageBox.Show("Fields Cannot be Empty");
                }
            }
            
        }
        /// <summary>
        /// Event Handler Method For saving Data
        /// </summary>
        /// <param name="members"></param>
        public void Event_OnChanged(ObservableCollection<Member> members)
        {
            string Fname="", Lname="", Email="";
            foreach (Member m in members)
            {
                Fname = m.Firstname;
                Lname = m.Lastname;
                Email = m.Email;
            }
            m = new Member(Fname, Lname, Email);
            message = new MessageMember() { member = m, MessageType = (int)MessageMember.MessageTypeList.AddList };
            Messenger.Default.Send<MessageMember>(message);
            
        }
        /// <summary>
        /// Property For FirstName
        /// </summary>
        public string EnteredFName
        {
            
             get
            {
                return enteredFName;
            }
            set
            {
                string input = value.ToString();
                bool rt = Regex.IsMatch(input, "[a-zA-Z]+$");
                try
                {
                    if(value.Length>text_limit)
                    {
                        throw new Exception("Invalid FirstName");
                    }
                    if(!rt)
                    {
                        throw new Exception("Enter Valid Name");
                    }
                    enteredFName = value;
                    RaisePropertyChanged("EnteredFName");
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "Enter Valid Name", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                
            }
        }

        /// <summary>
        /// Peoperty for Last Name
        /// </summary>
        public string EnteredLName
        {
            get
            {
                return enteredLName;
            }
            set
            {
                try
                {
                    string input = value.ToString();
                    bool rt = Regex.IsMatch(input, "[a-zA-Z]+$");
                    if (value.Length > 25)
                    {
                        throw new ArgumentException("Too Long");
                    }
                    if (value.Length == 0)
                    {
                        throw new NullReferenceException("Invalid Entry");
                    }
                    if (!rt)
                    {
                        throw new Exception("Enter Valid Name");
                    }
                    enteredLName = value;
                    RaisePropertyChanged("EnteredLName");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Enter Valid Name", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        /// <summary>
        /// Property for Email
        /// </summary>
        public string EnteredEmailId
        {
            get
            {
                return enteredEmailId;
            }
            set
            {
                try
                {
                    if (value.Length > text_limit)
                    {
                        throw new ArgumentException("Too Long");
                    }
                    if (value.Length == 0)
                    {
                        throw new NullReferenceException("Invalid Entry");
                    }
                    if (value.IndexOf('@') == -1 || value.IndexOf('.') == -1)
                    {
                        throw new FormatException("Enter Valid Email Id");
                    }
                    enteredEmailId = value;
                    RaisePropertyChanged("EnteredEmailId");
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "Enter Valid Email Id", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

      
    }
}
