using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CECSAssignment3.Message;
using CECSAssignment3.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using System.Text.RegularExpressions;

namespace CECSAssignment3.ViewModel
{
    /// <summary>
    /// Update View Model Class 
    /// </summary>
    public class UpdateViewModel : ViewModelBase
    {
        private string enteredFname;
        private string enteredLname;
        private string enteredemailID;
        private Member currentMember;
        private int text_limit=25;

        /// <summary>
        /// Command Binding of Update Button
        /// </summary>
        public ICommand UpdateCommand { get; private set; }

        /// <summary>
        /// Command Binding of Cancel Button
        /// </summary>
        public ICommand CancelCommand { get; private set; }


        /// <summary>
        /// Update View Model Constructor
        /// </summary>
        public UpdateViewModel()
        {
            UpdateCommand = new RelayCommand<ICloseable>(UpdateMethod);
            CancelCommand = new RelayCommand<ICloseable>(CancelMethod);
        }


        /// <summary>
        /// Cancel Method Operation
        /// </summary>
        /// <param name="window">For closing window</param>
        private void CancelMethod(ICloseable window)
        {
            if (window != null)
            {
                window.Close();
            }
        }

        /// <summary>
        /// To Store the Update List
        /// </summary>
        /// <param name="m"></param>
        internal void SetProperties(Member m)
        {
            currentMember = m;
            EnteredFname = m.Firstname;
            EnteredLname = m.Lastname;
            EnteredemailId = m.Email;
        }

        /// <summary>
        /// Update Method Operation
        /// </summary>
        /// <param name="window"></param>
        private void UpdateMethod(ICloseable window)
        {
            if (EnteredFname == null || EnteredLname == null || EnteredemailId == null)
            {
                MessageBox.Show("Fields cannot be empty");
            }
            else
            {
                
                MessageMember message = new MessageMember();
                message.memberN = CurrentMember;
                message.member = new Member(EnteredFname, EnteredLname, EnteredemailId);
                message.MessageType = (int)MessageMember.MessageTypeList.UpdateList;
                Messenger.Default.Send<MessageMember>(message);
                window.Close();
            }
        }


        /// <summary>
        /// Property for FirstName
        /// </summary>
        public string EnteredFname
        {
            get
            {
                return enteredFname;
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
                    enteredFname = value;
                    RaisePropertyChanged("EnteredFname");
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "Enter Valid First Name", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        /// <summary>
        /// Property For Last Name
        /// </summary>
        public string EnteredLname
        {
            get
            {
                return enteredLname;
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

                    enteredLname = value;
                    RaisePropertyChanged("EnteredLname");
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "Enter Valid Last Name", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                
            }
        }


        /// <summary>
        /// Property for Email
        /// </summary>
        public string EnteredemailId
        {
            get
            {
                return enteredemailID;
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
                    enteredemailID = value;
                    RaisePropertyChanged("EnteredemailId");
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "Enter Valid Email Id", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }


        /// <summary>
        /// Property for storing Selected Item
        /// </summary>
        public Member CurrentMember
        {
            get
            {
                return currentMember;
            }
            set
            {
                currentMember = value;
            }
        }
    }
}
